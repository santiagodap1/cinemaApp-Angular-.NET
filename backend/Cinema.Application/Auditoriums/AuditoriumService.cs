using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Abstractions.Data;
using Cinema.Application.Common;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Application.Auditoriums;

public sealed class AuditoriumService : IAuditoriumService
{
    private readonly IApplicationDbContext _dbContext;

    public AuditoriumService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<AuditoriumResponse>> GetAllAsync(Guid? cinemaSiteId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Auditoriums.AsNoTracking();

        if (cinemaSiteId.HasValue)
        {
            query = query.Where(a => a.CinemaSiteId == cinemaSiteId.Value);
        }

        return await query
            .OrderBy(a => a.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AuditoriumResponse(a.Id, a.CinemaSiteId, a.Name, a.Capacity))
            .ToListAsync(cancellationToken);
    }

    public async Task<AuditoriumResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Auditoriums
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new AuditoriumResponse(a.Id, a.CinemaSiteId, a.Name, a.Capacity))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ServiceResult<AuditoriumResponse>> CreateAsync(CreateAuditoriumRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var siteExists = await _dbContext.CinemaSites
            .AnyAsync(site => site.Id == request.CinemaSiteId, cancellationToken);

        if (!siteExists)
        {
            return ServiceResult<AuditoriumResponse>.NotFound("Cinema site not found.");
        }

        var auditorium = new Auditorium(request.CinemaSiteId, request.Name, request.Capacity);
        _dbContext.Auditoriums.Add(auditorium);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new AuditoriumResponse(auditorium.Id, auditorium.CinemaSiteId, auditorium.Name, auditorium.Capacity);
        return ServiceResult<AuditoriumResponse>.Success(response);
    }

    public async Task<ServiceResult<AuditoriumResponse>> UpdateAsync(Guid id, UpdateAuditoriumRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var auditorium = await _dbContext.Auditoriums
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (auditorium is null)
        {
            return ServiceResult<AuditoriumResponse>.NotFound("Auditorium not found.");
        }

        auditorium.UpdateName(request.Name);
        auditorium.UpdateCapacity(request.Capacity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new AuditoriumResponse(auditorium.Id, auditorium.CinemaSiteId, auditorium.Name, auditorium.Capacity);
        return ServiceResult<AuditoriumResponse>.Success(response);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var auditorium = await _dbContext.Auditoriums
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (auditorium is null)
        {
            return ServiceResult<bool>.NotFound("Auditorium not found.");
        }

        _dbContext.Auditoriums.Remove(auditorium);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.Success(true);
    }
}
