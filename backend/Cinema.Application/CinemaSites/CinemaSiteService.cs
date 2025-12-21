using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Abstractions.Data;
using Cinema.Application.Common;
using Cinema.Domain.Entities;
using Cinema.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Application.CinemaSites;

public sealed class CinemaSiteService : ICinemaSiteService
{
    private readonly IApplicationDbContext _dbContext;

    public CinemaSiteService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<CinemaSiteResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbContext.CinemaSites
            .AsNoTracking()
            .OrderBy(site => site.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(site => new CinemaSiteResponse(
                site.Id,
                site.Name,
                new AddressDto(
                    site.Address.Street,
                    site.Address.City,
                    site.Address.State,
                    site.Address.Country,
                    site.Address.PostalCode)))
            .ToListAsync(cancellationToken);
    }

    public async Task<CinemaSiteResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.CinemaSites
            .AsNoTracking()
            .Where(site => site.Id == id)
            .Select(site => new CinemaSiteResponse(
                site.Id,
                site.Name,
                new AddressDto(
                    site.Address.Street,
                    site.Address.City,
                    site.Address.State,
                    site.Address.Country,
                    site.Address.PostalCode)))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ServiceResult<CinemaSiteResponse>> CreateAsync(CreateCinemaSiteRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Address is null)
        {
            return ServiceResult<CinemaSiteResponse>.Validation("Address is required.");
        }

        var address = new Address(
            request.Address.Street,
            request.Address.City,
            request.Address.State,
            request.Address.Country,
            request.Address.PostalCode);

        var site = new CinemaSite(request.Name, address);
        _dbContext.CinemaSites.Add(site);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new CinemaSiteResponse(
            site.Id,
            site.Name,
            new AddressDto(
                site.Address.Street,
                site.Address.City,
                site.Address.State,
                site.Address.Country,
                site.Address.PostalCode));

        return ServiceResult<CinemaSiteResponse>.Success(response);
    }

    public async Task<ServiceResult<CinemaSiteResponse>> UpdateAsync(Guid id, UpdateCinemaSiteRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.Address is null)
        {
            return ServiceResult<CinemaSiteResponse>.Validation("Address is required.");
        }

        var site = await _dbContext.CinemaSites
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (site is null)
        {
            return ServiceResult<CinemaSiteResponse>.NotFound("Cinema site not found.");
        }

        site.UpdateName(request.Name);
        site.UpdateAddress(new Address(
            request.Address.Street,
            request.Address.City,
            request.Address.State,
            request.Address.Country,
            request.Address.PostalCode));

        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new CinemaSiteResponse(
            site.Id,
            site.Name,
            new AddressDto(
                site.Address.Street,
                site.Address.City,
                site.Address.State,
                site.Address.Country,
                site.Address.PostalCode));

        return ServiceResult<CinemaSiteResponse>.Success(response);
    }

    public async Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var site = await _dbContext.CinemaSites
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (site is null)
        {
            return ServiceResult<bool>.NotFound("Cinema site not found.");
        }

        _dbContext.CinemaSites.Remove(site);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ServiceResult<bool>.Success(true);
    }
}
