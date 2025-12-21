using System;
using System.Collections.Generic;
using Cinema.Domain.Common;

namespace Cinema.Domain.Entities;

public sealed class Customer : Entity
{
    private readonly List<Reservation> _reservations = new();

    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();

    private Customer()
    {
    }

    public Customer(string fullName, string email)
    {
        FullName = Require(fullName, nameof(fullName));
        Email = Require(email, nameof(email));
    }

    public void AddReservation(Reservation reservation)
    {
        if (reservation is null)
        {
            throw new ArgumentNullException(nameof(reservation));
        }

        _reservations.Add(reservation);
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
