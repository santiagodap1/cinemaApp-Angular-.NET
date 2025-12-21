using System;

namespace Cinema.Application.SeatMaps;

public sealed record SeatMapItem(
    Guid SeatId,
    string Row,
    int Number,
    SeatAvailabilityStatus Status);
