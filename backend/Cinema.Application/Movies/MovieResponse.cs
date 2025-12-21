using System;

namespace Cinema.Application.Movies;

public sealed record MovieResponse(
    Guid Id,
    string Title,
    int DurationMinutes,
    string Rating,
    string Synopsis,
    string Director,
    string Cast,
    string Language,
    string Country,
    DateOnly? ReleaseDate);
