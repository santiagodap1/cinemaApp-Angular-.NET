using System;
using Cinema.Domain.Entities;

public sealed record CreateScreeningRequest(
    Guid MovieId,
    Guid AuditoriumId,
    DateTimeOffset StartsAt,
    DateTimeOffset EndsAt,
    MovieFormat Format);
