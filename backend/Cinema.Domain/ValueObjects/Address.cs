using System;

namespace Cinema.Domain.ValueObjects;

public sealed record Address
{
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;

    private Address()
    {
    }

    public Address(string street, string city, string state, string country, string postalCode)
    {
        Street = Require(street, nameof(street));
        City = Require(city, nameof(city));
        State = Require(state, nameof(state));
        Country = Require(country, nameof(country));
        PostalCode = Require(postalCode, nameof(postalCode));
    }

    private static string Require(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{field} is required.", field);
        }

        return value.Trim();
    }
}
