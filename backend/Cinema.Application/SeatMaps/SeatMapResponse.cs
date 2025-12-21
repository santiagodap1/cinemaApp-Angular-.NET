using System;
using System.Collections.Generic;

namespace Cinema.Application.SeatMaps;

public sealed record SeatMapResponse(
    Guid AuditoriumId,
    int Capacity,
    Guid ScreeningId,
    DateTimeOffset StartsAt,
    IReadOnlyList<SeatMapItem> Seats);
