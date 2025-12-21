using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Common;

namespace Cinema.Application.Seats;

public interface ISeatService
{
    Task<IReadOnlyList<SeatResponse>> GetByAuditoriumAsync(Guid auditoriumId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<ServiceResult<SeatResponse>> CreateAsync(CreateSeatRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> DeleteAsync(Guid seatId, CancellationToken cancellationToken = default);
}
