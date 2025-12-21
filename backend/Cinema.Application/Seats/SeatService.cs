using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Abstractions.Data;
using Cinema.Application.Common;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Application.Seats;

public sealed class SeatService : ISeatService
{
    private readonly IApplicationDbContext _dbContext;

    public SeatService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<SeatResponse>> GetByAuditoriumAsync(Guid auditoriumId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Seats
            .AsNoTracking()
            .Where(seat => seat.AuditoriumId == auditoriumId)
            .OrderBy(seat => seat.Row)
            .ThenBy(seat => seat.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(seat => new SeatResponse(seat.Id, seat.AuditoriumId, seat.Row, seat.Number))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceResult<SeatResponse>> CreateAsync(CreateSeatRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var auditoriumExists = await _dbContext.Auditoriums
            .AnyAsync(a => a.Id == request.AuditoriumId, cancellationToken);

        if (!auditoriumExists)
        {
            return ServiceResult<SeatResponse>.NotFound("Auditorium not found.");
        }

        var row = request.Row.Trim();
        var alreadyExists = await _dbContext.Seats
            .AnyAsync(seat => seat.AuditoriumId == request.AuditoriumId &&
                              seat.Row == row &&
                              seat.Number == request.Number, cancellationToken);

        if (alreadyExists)
        {
            return ServiceResult<SeatResponse>.Conflict("Seat already exists.");
        }

        var seat = new Seat(request.AuditoriumId, row, request.Number);
        _dbContext.Seats.Add(seat);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new SeatResponse(seat.Id, seat.AuditoriumId, seat.Row, seat.Number);
        return ServiceResult<SeatResponse>.Success(response);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid seatId, CancellationToken cancellationToken = default)
    {
        var seat = await _dbContext.Seats
            .FirstOrDefaultAsync(s => s.Id == seatId, cancellationToken);

        if (seat is null)
        {
            return ServiceResult<bool>.NotFound("Seat not found.");
        }

        _dbContext.Seats.Remove(seat);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.Success(true);
    }
}
