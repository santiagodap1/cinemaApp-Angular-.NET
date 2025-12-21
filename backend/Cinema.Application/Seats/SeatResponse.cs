using System;

namespace Cinema.Application.Seats;

public sealed record SeatResponse(
    Guid Id,
    Guid AuditoriumId,
    string Row,
    int Number);
