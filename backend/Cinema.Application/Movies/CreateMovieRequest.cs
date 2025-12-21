namespace Cinema.Application.Movies;

public sealed record CreateMovieRequest(
    string Title,
    int DurationMinutes,
    string Rating,
    string? Synopsis,
    string? Director,
    string? Cast,
    string? Language,
    string? Country,
    DateOnly? ReleaseDate);
