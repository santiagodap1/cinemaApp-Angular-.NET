using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cinema.Application.SeatMaps;

public interface ISeatMapService
{
    Task<SeatMapResponse?> GetByScreeningAsync(Guid screeningId, CancellationToken cancellationToken = default);
}
