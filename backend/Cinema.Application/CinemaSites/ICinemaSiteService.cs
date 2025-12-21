using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Common;

namespace Cinema.Application.CinemaSites;

public interface ICinemaSiteService
{
    Task<IReadOnlyList<CinemaSiteResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<CinemaSiteResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ServiceResult<CinemaSiteResponse>> CreateAsync(CreateCinemaSiteRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<CinemaSiteResponse>> UpdateAsync(Guid id, UpdateCinemaSiteRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
