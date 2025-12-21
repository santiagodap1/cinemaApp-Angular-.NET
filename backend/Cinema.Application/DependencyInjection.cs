using Microsoft.Extensions.DependencyInjection;

namespace Cinema.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Movies.IMovieService, Movies.MovieService>();
        services.AddScoped<Screenings.IScreeningService, Screenings.ScreeningService>();
        services.AddScoped<Reservations.IReservationService, Reservations.ReservationService>();
        services.AddScoped<SeatMaps.ISeatMapService, SeatMaps.SeatMapService>();
        services.AddScoped<CinemaSites.ICinemaSiteService, CinemaSites.CinemaSiteService>();
        services.AddScoped<Auditoriums.IAuditoriumService, Auditoriums.AuditoriumService>();
        services.AddScoped<Seats.ISeatService, Seats.SeatService>();

        return services;
    }
}
