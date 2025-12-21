namespace Cinema.Application.CinemaSites;

public sealed record AddressDto(
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode);
