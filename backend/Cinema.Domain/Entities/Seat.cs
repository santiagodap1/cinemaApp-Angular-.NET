using System;
using Cinema.Domain.Common;

namespace Cinema.Domain.Entities;

public sealed class Seat : Entity
{
    public Guid AuditoriumId { get; private set; }
    public string Row { get; private set; } = string.Empty;
    public int Number { get; private set; }
    public Auditorium Auditorium { get; private set; } = null!;

    private Seat()
    {
    }

    public Seat(Guid auditoriumId, string row, int number)
    {
        AuditoriumId = auditoriumId;
        Row = Require(row, nameof(row));
        Number = number > 0 ? number : throw new ArgumentOutOfRangeException(nameof(number));
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
