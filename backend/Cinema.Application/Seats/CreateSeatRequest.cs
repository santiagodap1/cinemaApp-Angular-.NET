using System;

namespace Cinema.Application.Seats;

public sealed record CreateSeatRequest(
    Guid AuditoriumId,
    string Row,
    int Number);
