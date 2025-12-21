using System;
using System.Collections.Generic;
using Cinema.Domain.Entities;

namespace Cinema.Application.Reservations;

public sealed record ReservationResponse(
    Guid Id,
    Guid ScreeningId,
    Guid CustomerId,
    ReservationStatus Status,
    decimal TotalAmount,
    DateTimeOffset ReservedAt,
    IReadOnlyList<Guid> SeatIds);
