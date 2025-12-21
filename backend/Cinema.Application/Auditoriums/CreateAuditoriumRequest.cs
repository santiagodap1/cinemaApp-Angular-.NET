using System;

namespace Cinema.Application.Auditoriums;

public sealed record CreateAuditoriumRequest(
    Guid CinemaSiteId,
    string Name,
    int Capacity);
