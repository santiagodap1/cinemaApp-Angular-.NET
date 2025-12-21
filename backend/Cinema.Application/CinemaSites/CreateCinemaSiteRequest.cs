namespace Cinema.Application.CinemaSites;

public sealed record CreateCinemaSiteRequest(
    string Name,
    AddressDto Address);
