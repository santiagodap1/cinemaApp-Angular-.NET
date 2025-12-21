using System;

namespace Cinema.Application.Screenings;

public sealed record ScreeningOccupancyResponse(
    Guid ScreeningId,
    int Capacity,
    int ReservedCount,
    int HeldCount,
    int AvailableCount);
