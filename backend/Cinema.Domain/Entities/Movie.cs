using System;
using System.Collections.Generic;
using Cinema.Domain.Common;

namespace Cinema.Domain.Entities;

public sealed class Movie : Entity
{
    private readonly List<Screening> _screenings = new();

    public string Title { get; private set; } = string.Empty;
    public int DurationMinutes { get; private set; }
    public string Rating { get; private set; } = string.Empty;
    public string Synopsis { get; private set; } = string.Empty;
    public string Director { get; private set; } = string.Empty;
    public string Cast { get; private set; } = string.Empty;
    public string Language { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public DateOnly? ReleaseDate { get; private set; }
    public IReadOnlyCollection<Screening> Screenings => _screenings.AsReadOnly();

    private Movie()
    {
    }

    public Movie(
        string title,
        int durationMinutes,
        string rating,
        string? synopsis = null,
        string? director = null,
        string? cast = null,
        string? language = null,
        string? country = null,
        DateOnly? releaseDate = null)
    {
        Title = Require(title, nameof(title));
        DurationMinutes = durationMinutes > 0 ? durationMinutes : throw new ArgumentOutOfRangeException(nameof(durationMinutes));
        Rating = Require(rating, nameof(rating));
        Synopsis = Optional(synopsis);
        Director = Optional(director);
        Cast = Optional(cast);
        Language = Optional(language);
        Country = Optional(country);
        ReleaseDate = releaseDate;
    }

    public void AddScreening(Screening screening)
    {
        if (screening is null)
        {
            throw new ArgumentNullException(nameof(screening));
        }

        _screenings.Add(screening);
    }

    private static string Require(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{field} is required.", field);
        }

        return value.Trim();
    }

    private static string Optional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }
}
