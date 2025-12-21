namespace Cinema.Application.Auditoriums;

public sealed record UpdateAuditoriumRequest(
    string Name,
    int Capacity);
