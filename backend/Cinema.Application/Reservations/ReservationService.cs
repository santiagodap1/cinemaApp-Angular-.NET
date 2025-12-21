using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Abstractions.Data;
using Cinema.Application.Abstractions.Pricing;
using Cinema.Application.Abstractions.Time;
using Cinema.Application.Common;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Application.Reservations;

public sealed class ReservationService : IReservationService
{
    private static readonly TimeSpan HoldDuration = TimeSpan.FromMinutes(10);

    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPriceCalculator _priceCalculator;

    public ReservationService(
        IApplicationDbContext dbContext,
        IDateTimeProvider dateTimeProvider,
        IPriceCalculator priceCalculator)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _priceCalculator = priceCalculator;
    }

    public async Task<ServiceResult<ReservationResponse>> CreateAsync(CreateReservationRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.SeatIds is null || request.SeatIds.Count == 0)
        {
            return ServiceResult<ReservationResponse>.Validation("At least one seat is required.");
        }

        var screening = await _dbContext.Screenings
            .AsNoTracking()
            .Where(s => s.Id == request.ScreeningId)
            .Select(s => new { s.Id, s.AuditoriumId, s.StartsAt })
            .FirstOrDefaultAsync(cancellationToken);

        if (screening is null)
        {
            return ServiceResult<ReservationResponse>.NotFound("Screening not found.");
        }

        var seatIds = request.SeatIds.Distinct().ToList();
        var validSeatIds = await _dbContext.Seats
            .AsNoTracking()
            .Where(seat => seat.AuditoriumId == screening.AuditoriumId)
            .Where(seat => seatIds.Contains(seat.Id))
            .Select(seat => seat.Id)
            .ToListAsync(cancellationToken);

        if (validSeatIds.Count != seatIds.Count)
        {
            return ServiceResult<ReservationResponse>.Validation("One or more seats are invalid for this auditorium.");
        }

        var activeReservationIds = await _dbContext.Reservations
            .AsNoTracking()
            .Where(reservation => reservation.ScreeningId == screening.Id)
            .Where(reservation => reservation.Status == ReservationStatus.Confirmed ||
                                  (reservation.Status == ReservationStatus.Pending &&
                                   reservation.ReservedAt > _dateTimeProvider.UtcNow.Subtract(HoldDuration)))
            .Select(reservation => reservation.Id)
            .ToListAsync(cancellationToken);

        if (activeReservationIds.Count > 0)
        {
            var reservedSeatIds = await _dbContext.ReservationSeats
                .AsNoTracking()
                .Where(reservationSeat => activeReservationIds.Contains(reservationSeat.ReservationId))
                .Select(reservationSeat => reservationSeat.SeatId)
                .ToListAsync(cancellationToken);

            if (reservedSeatIds.Intersect(seatIds).Any())
            {
                return ServiceResult<ReservationResponse>.Conflict("One or more seats are already reserved.");
            }
        }

        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(c => c.Email == request.CustomerEmail, cancellationToken);

        if (customer is null)
        {
            customer = new Customer(request.CustomerFullName, request.CustomerEmail);
            _dbContext.Customers.Add(customer);
        }

        var reservedAt = _dateTimeProvider.UtcNow;
        var seatPrice = _priceCalculator.CalculateSeatPrice(screening.StartsAt);
        var totalAmount = decimal.Round(seatPrice * seatIds.Count, 2, MidpointRounding.AwayFromZero);

        var reservation = new Reservation(screening.Id, customer.Id, totalAmount, reservedAt);

        foreach (var seatId in seatIds)
        {
            reservation.AddSeat(new ReservationSeat(reservation.Id, seatId));
        }

        _dbContext.Reservations.Add(reservation);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new ReservationResponse(
            reservation.Id,
            reservation.ScreeningId,
            reservation.CustomerId,
            reservation.Status,
            reservation.TotalAmount,
            reservation.ReservedAt,
            seatIds);

        return ServiceResult<ReservationResponse>.Success(response);
    }

    public async Task<ServiceResult<ReservationResponse>> ConfirmAsync(Guid reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await _dbContext.Reservations
            .Include(r => r.Seats)
            .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);

        if (reservation is null)
        {
            return ServiceResult<ReservationResponse>.NotFound("Reservation not found.");
        }

        if (reservation.Status == ReservationStatus.Expired || reservation.Status == ReservationStatus.Cancelled)
        {
            return ServiceResult<ReservationResponse>.Conflict("Reservation is not active.");
        }

        if (reservation.Status == ReservationStatus.Pending &&
            reservation.ReservedAt <= _dateTimeProvider.UtcNow.Subtract(HoldDuration))
        {
            reservation.Expire();
            await _dbContext.SaveChangesAsync(cancellationToken);
            return ServiceResult<ReservationResponse>.Conflict("Reservation hold expired.");
        }

        reservation.Confirm();
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new ReservationResponse(
            reservation.Id,
            reservation.ScreeningId,
            reservation.CustomerId,
            reservation.Status,
            reservation.TotalAmount,
            reservation.ReservedAt,
            reservation.Seats.Select(seat => seat.SeatId).ToList());

        return ServiceResult<ReservationResponse>.Success(response);
    }

    public async Task<ServiceResult<ReservationResponse>> CancelAsync(Guid reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await _dbContext.Reservations
            .Include(r => r.Seats)
            .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);

        if (reservation is null)
        {
            return ServiceResult<ReservationResponse>.NotFound("Reservation not found.");
        }

        if (reservation.Status == ReservationStatus.Cancelled)
        {
            return ServiceResult<ReservationResponse>.Conflict("Reservation is already cancelled.");
        }

        reservation.Cancel();
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new ReservationResponse(
            reservation.Id,
            reservation.ScreeningId,
            reservation.CustomerId,
            reservation.Status,
            reservation.TotalAmount,
            reservation.ReservedAt,
            reservation.Seats.Select(seat => seat.SeatId).ToList());

        return ServiceResult<ReservationResponse>.Success(response);
    }
}
