using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Common;

namespace Cinema.Application.Screenings;

public interface IScreeningService
{
    Task<IReadOnlyList<ScreeningResponse>> GetAllAsync(Guid? movieId, Guid? cinemaSiteId, DateOnly? date, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ScreeningResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ScreeningOccupancyResponse?> GetOccupancyAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResult<ScreeningResponse>> CreateAsync(CreateScreeningRequest request, CancellationToken cancellationToken = default);
}
