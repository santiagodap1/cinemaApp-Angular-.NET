namespace Cinema.Application.CinemaSites;

public sealed record UpdateCinemaSiteRequest(
    string Name,
    AddressDto Address);
