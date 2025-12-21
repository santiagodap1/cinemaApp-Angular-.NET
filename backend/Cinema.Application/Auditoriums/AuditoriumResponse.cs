using System;

namespace Cinema.Application.Auditoriums;

public sealed record AuditoriumResponse(
    Guid Id,
    Guid CinemaSiteId,
    string Name,
    int Capacity);
