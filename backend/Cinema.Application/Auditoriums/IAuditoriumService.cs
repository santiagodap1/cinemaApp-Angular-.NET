using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Common;

namespace Cinema.Application.Auditoriums;

public interface IAuditoriumService
{
    Task<IReadOnlyList<AuditoriumResponse>> GetAllAsync(Guid? cinemaSiteId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<AuditoriumResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResult<AuditoriumResponse>> CreateAsync(CreateAuditoriumRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<AuditoriumResponse>> UpdateAsync(Guid id, UpdateAuditoriumRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
