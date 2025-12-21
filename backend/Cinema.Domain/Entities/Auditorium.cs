using System;
using System.Collections.Generic;
using Cinema.Domain.Common;

namespace Cinema.Domain.Entities;

public sealed class Auditorium : Entity
{
    private readonly List<Seat> _seats = new();

    public Guid CinemaSiteId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Capacity { get; private set; }
    public CinemaSite CinemaSite { get; private set; } = null!;
    public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();

    private Auditorium()
    {
    }

    public Auditorium(Guid cinemaSiteId, string name, int capacity)
    {
        CinemaSiteId = cinemaSiteId;
        Name = Require(name, nameof(name));
        Capacity = capacity > 0 ? capacity : throw new ArgumentOutOfRangeException(nameof(capacity));
    }

    public void AddSeat(Seat seat)
    {
        if (seat is null)
        {
            throw new ArgumentNullException(nameof(seat));
        }

        _seats.Add(seat);
    }

    public void UpdateName(string name)
    {
        Name = Require(name, nameof(name));
    }

    public void UpdateCapacity(int capacity)
    {
        Capacity = capacity > 0 ? capacity : throw new ArgumentOutOfRangeException(nameof(capacity));
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
