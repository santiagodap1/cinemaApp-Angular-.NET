using System;
using System.Collections.Generic;
using Cinema.Domain.Common;

namespace Cinema.Domain.Entities;

public sealed class Screening : Entity
{
    private readonly List<Reservation> _reservations = new();

    public Guid MovieId { get; private set; }
    public Guid AuditoriumId { get; private set; }
    public DateTimeOffset StartsAt { get; private set; }
    public DateTimeOffset EndsAt { get; private set; }
    public MovieFormat Format { get; private set; }

    public Movie Movie { get; private set; } = null!;
    public Auditorium Auditorium { get; private set; } = null!;
    public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();

    private Screening()
    {
    }

    public Screening(Guid movieId, Guid auditoriumId, DateTimeOffset startsAt, DateTimeOffset endsAt, MovieFormat format = MovieFormat.Format2D)
    {
        if (endsAt <= startsAt)
        {
            throw new ArgumentException("EndsAt must be after StartsAt.", nameof(endsAt));
        }

        MovieId = movieId;
        AuditoriumId = auditoriumId;
        StartsAt = startsAt;
        EndsAt = endsAt;
        Format = format;
    }

    public void AddReservation(Reservation reservation)
    {
        if (reservation is null)
        {
            throw new ArgumentNullException(nameof(reservation));
        }

        _reservations.Add(reservation);
    }
}
