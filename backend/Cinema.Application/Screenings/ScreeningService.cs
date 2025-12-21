using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Abstractions.Data;
using Cinema.Application.Common;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Application.Screenings;

public sealed class ScreeningService : IScreeningService
{
    private readonly IApplicationDbContext _dbContext;

    public ScreeningService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ScreeningResponse>> GetAllAsync(Guid? movieId, Guid? cinemaSiteId, DateOnly? date, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Screenings.AsNoTracking();

        if (movieId.HasValue)
        {
            query = query.Where(screening => screening.MovieId == movieId.Value);
        }

        if (cinemaSiteId.HasValue)
        {
            query = query.Where(screening => screening.Auditorium.CinemaSiteId == cinemaSiteId.Value);
        }

        if (date.HasValue)
        {
            var start = new DateTimeOffset(date.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
            var end = start.AddDays(1);
            query = query.Where(screening => screening.StartsAt >= start && screening.StartsAt < end);
        }

        return await query
            .OrderBy(screening => screening.StartsAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(screening => new ScreeningResponse(
                screening.Id,
                screening.MovieId,
                screening.AuditoriumId,
                screening.StartsAt,
                screening.EndsAt,
                screening.Format))
            .ToListAsync(cancellationToken);
    }

    public async Task<ScreeningResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Screenings
            .AsNoTracking()
            .Where(screening => screening.Id == id)
            .Select(screening => new ScreeningResponse(
                screening.Id,
                screening.MovieId,
                screening.AuditoriumId,
                screening.StartsAt,
                screening.EndsAt,
                screening.Format))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ScreeningOccupancyResponse?> GetOccupancyAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var screening = await _dbContext.Screenings
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new { s.Id, s.Auditorium.Capacity })
            .FirstOrDefaultAsync(cancellationToken);

        if (screening is null)
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;
        var holdThreshold = now.Subtract(TimeSpan.FromMinutes(10));

        var reservationStatuses = await _dbContext.Reservations
            .AsNoTracking()
            .Where(r => r.ScreeningId == id)
            .Select(r => new { r.Id, r.Status, r.ReservedAt })
            .ToListAsync(cancellationToken);

        var activeReservationIds = reservationStatuses
            .Where(r => r.Status == ReservationStatus.Confirmed ||
                        (r.Status == ReservationStatus.Pending && r.ReservedAt > holdThreshold))
            .Select(r => new { r.Id, r.Status })
            .ToList();

        var reservedSeatIds = await _dbContext.ReservationSeats
            .AsNoTracking()
            .Where(rs => activeReservationIds.Select(a => a.Id).Contains(rs.ReservationId))
            .Select(rs => new { rs.SeatId, rs.ReservationId })
            .ToListAsync(cancellationToken);

        var statusLookup = activeReservationIds.ToDictionary(r => r.Id, r => r.Status);

        var reservedCount = reservedSeatIds.Count(rs => statusLookup[rs.ReservationId] == ReservationStatus.Confirmed);
        var heldCount = reservedSeatIds.Count(rs => statusLookup[rs.ReservationId] == ReservationStatus.Pending);
        var available = Math.Max(0, screening.Capacity - reservedCount - heldCount);

        return new ScreeningOccupancyResponse(screening.Id, screening.Capacity, reservedCount, heldCount, available);
    }

    public async Task<ServiceResult<ScreeningResponse>> CreateAsync(CreateScreeningRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var movieExists = await _dbContext.Movies
            .AnyAsync(movie => movie.Id == request.MovieId, cancellationToken);

        if (!movieExists)
        {
            return ServiceResult<ScreeningResponse>.NotFound("Movie not found.");
        }

        var auditoriumExists = await _dbContext.Auditoriums
            .AnyAsync(auditorium => auditorium.Id == request.AuditoriumId, cancellationToken);

        if (!auditoriumExists)
        {
            return ServiceResult<ScreeningResponse>.NotFound("Auditorium not found.");
        }

        var hasOverlap = await _dbContext.Screenings
            .AsNoTracking()
            .Where(screening => screening.AuditoriumId == request.AuditoriumId)
            .AnyAsync(screening => screening.StartsAt < request.EndsAt && screening.EndsAt > request.StartsAt, cancellationToken);

        if (hasOverlap)
        {
            return ServiceResult<ScreeningResponse>.Conflict("Screening overlaps with an existing schedule.");
        }

        var screening = new Screening(
            request.MovieId,
            request.AuditoriumId,
            request.StartsAt,
            request.EndsAt,
            request.Format);

        _dbContext.Screenings.Add(screening);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new ScreeningResponse(
            screening.Id,
            screening.MovieId,
            screening.AuditoriumId,
            screening.StartsAt,
            screening.EndsAt,
            screening.Format);

        return ServiceResult<ScreeningResponse>.Success(response);
    }
}
