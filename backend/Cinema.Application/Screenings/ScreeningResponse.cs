using System;
using Cinema.Domain.Entities;

public sealed record ScreeningResponse(
    Guid Id,
    Guid MovieId,
    Guid AuditoriumId,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt,
    MovieFormat Format);
