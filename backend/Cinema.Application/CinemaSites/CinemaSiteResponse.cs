using System;

namespace Cinema.Application.CinemaSites;

public sealed record CinemaSiteResponse(
    Guid Id,
    string Name,
    AddressDto Address);
