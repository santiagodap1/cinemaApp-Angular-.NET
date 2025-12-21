using System;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Common;

namespace Cinema.Application.Reservations;

public interface IReservationService
{
    Task<ServiceResult<ReservationResponse>> CreateAsync(CreateReservationRequest request, CancellationToken cancellationToken = default);
    Task<ServiceResult<ReservationResponse>> ConfirmAsync(Guid reservationId, CancellationToken cancellationToken = default);
    Task<ServiceResult<ReservationResponse>> CancelAsync(Guid reservationId, CancellationToken cancellationToken = default);
}
