using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Abstractions.Data;
using Cinema.Application.Abstractions.Time;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Application.SeatMaps;

public sealed class SeatMapService : ISeatMapService
{
    private static readonly TimeSpan HoldDuration = TimeSpan.FromMinutes(10);

    private readonly IApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SeatMapService(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<SeatMapResponse?> GetByScreeningAsync(Guid screeningId, CancellationToken cancellationToken = default)
    {
        var screening = await _dbContext.Screenings
            .AsNoTracking()
            .Where(s => s.Id == screeningId)
            .Select(s => new { s.Id, s.AuditoriumId, s.StartsAt, s.Auditorium.Capacity })
            .FirstOrDefaultAsync(cancellationToken);

        if (screening is null)
        {
            return null;
        }

        var seats = await _dbContext.Seats
            .AsNoTracking()
            .Where(seat => seat.AuditoriumId == screening.AuditoriumId)
            .OrderBy(seat => seat.Row)
            .ThenBy(seat => seat.Number)
            .Select(seat => new { seat.Id, seat.Row, seat.Number })
            .ToListAsync(cancellationToken);

        var now = _dateTimeProvider.UtcNow;
        var activeReservationIds = await _dbContext.Reservations
            .AsNoTracking()
            .Where(reservation => reservation.ScreeningId == screening.Id)
            .Where(reservation => reservation.Status == ReservationStatus.Confirmed ||
                                  (reservation.Status == ReservationStatus.Pending &&
                                   reservation.ReservedAt > now.Subtract(HoldDuration)))
            .Select(reservation => new { reservation.Id, reservation.Status })
            .ToListAsync(cancellationToken);

        var reservedSeatLookup = new Dictionary<Guid, SeatAvailabilityStatus>();

        if (activeReservationIds.Count > 0)
        {
            var activeReservationIdSet = activeReservationIds.Select(r => r.Id).ToList();
            var reservationSeatPairs = await _dbContext.ReservationSeats
                .AsNoTracking()
                .Where(rs => activeReservationIdSet.Contains(rs.ReservationId))
                .Select(rs => new { rs.ReservationId, rs.SeatId })
                .ToListAsync(cancellationToken);

            var statusLookup = activeReservationIds.ToDictionary(r => r.Id, r =>
                r.Status == ReservationStatus.Confirmed ? SeatAvailabilityStatus.Reserved : SeatAvailabilityStatus.Held);

            foreach (var pair in reservationSeatPairs)
            {
                reservedSeatLookup[pair.SeatId] = statusLookup[pair.ReservationId];
            }
        }

        var seatItems = seats
            .Select(seat => new SeatMapItem(
                seat.Id,
                seat.Row,
                seat.Number,
                reservedSeatLookup.TryGetValue(seat.Id, out var status) ? status : SeatAvailabilityStatus.Available))
            .ToList();

        return new SeatMapResponse(screening.AuditoriumId, screening.Capacity, screening.Id, screening.StartsAt, seatItems);
    }
}
