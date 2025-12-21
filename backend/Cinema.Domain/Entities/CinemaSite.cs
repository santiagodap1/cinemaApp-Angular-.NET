using System;
using System.Collections.Generic;
using Cinema.Domain.Common;
using Cinema.Domain.ValueObjects;

namespace Cinema.Domain.Entities;

public sealed class CinemaSite : Entity
{
    private readonly List<Auditorium> _auditoriums = new();

    public string Name { get; private set; } = string.Empty;
    public Address Address { get; private set; } = null!;
    public IReadOnlyCollection<Auditorium> Auditoriums => _auditoriums.AsReadOnly();

    private CinemaSite()
    {
    }

    public CinemaSite(string name, Address address)
    {
        Name = Require(name, nameof(name));
        Address = address ?? throw new ArgumentNullException(nameof(address));
    }

    public void AddAuditorium(Auditorium auditorium)
    {
        if (auditorium is null)
        {
            throw new ArgumentNullException(nameof(auditorium));
        }

        _auditoriums.Add(auditorium);
    }

    public void UpdateName(string name)
    {
        Name = Require(name, nameof(name));
    }

    public void UpdateAddress(Address address)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
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
