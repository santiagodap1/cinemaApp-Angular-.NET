using System;
using System.Collections.Generic;

namespace Cinema.Application.Reservations;

public sealed record CreateReservationRequest(
    Guid ScreeningId,
    string CustomerEmail,
    string CustomerFullName,
    IReadOnlyList<Guid> SeatIds);
