using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cinema.Tests.Integration;

public sealed class ApiWorkflowTests : IClassFixture<TestApiFactory>
{
    private readonly TestApiFactory _factory;

    public ApiWorkflowTests(TestApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetMovies_ReturnsSeededMovies()
    {
        var client = _factory.CreateClient();

        var movies = await client.GetFromJsonAsync<List<MovieResponseDto>>("/api/movies");

        Assert.NotNull(movies);
        Assert.NotEmpty(movies!);
    }

    [Fact]
    public async Task ReservationWorkflow_CreatesAndConfirms()
    {
        var client = _factory.CreateClient();

        var screenings = await client.GetFromJsonAsync<List<ScreeningResponseDto>>("/api/screenings");
        Assert.NotNull(screenings);
        var screening = screenings!.First();

        var seatMap = await client.GetFromJsonAsync<SeatMapResponseDto>($"/api/screenings/{screening.Id}/seats");
        Assert.NotNull(seatMap);

        var seat = seatMap!.Seats.First(s => s.Status == 0);

        var reservationRequest = new CreateReservationRequestDto(
            screening.Id,
            "demo@example.com",
            "Demo User",
            new List<Guid> { seat.SeatId });

        var createResponse = await client.PostAsJsonAsync("/api/reservations", reservationRequest);
        createResponse.EnsureSuccessStatusCode();
        var reservation = await createResponse.Content.ReadFromJsonAsync<ReservationResponseDto>();
        Assert.NotNull(reservation);

        var confirmResponse = await client.PostAsJsonAsync($"/api/reservations/{reservation!.Id}/confirm", new { });
        confirmResponse.EnsureSuccessStatusCode();
    }

    private sealed record MovieResponseDto(Guid Id, string Title, int DurationMinutes, string Rating);
    private sealed record ScreeningResponseDto(Guid Id, Guid MovieId, Guid AuditoriumId, DateTimeOffset StartsAt, DateTimeOffset EndsAt);

    private sealed record SeatMapResponseDto(Guid AuditoriumId, Guid ScreeningId, DateTimeOffset StartsAt, List<SeatMapItemDto> Seats);
    private sealed record SeatMapItemDto(Guid SeatId, string Row, int Number, int Status);

    private sealed record CreateReservationRequestDto(Guid ScreeningId, string CustomerEmail, string CustomerFullName, List<Guid> SeatIds);
    private sealed record ReservationResponseDto(Guid Id, Guid ScreeningId, Guid CustomerId, int Status, decimal TotalAmount, DateTimeOffset ReservedAt, List<Guid> SeatIds);
}
