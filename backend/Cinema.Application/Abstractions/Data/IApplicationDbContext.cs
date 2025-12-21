using System.Threading;
using System.Threading.Tasks;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<CinemaSite> CinemaSites { get; }
    DbSet<Auditorium> Auditoriums { get; }
    DbSet<Seat> Seats { get; }
    DbSet<Movie> Movies { get; }
    DbSet<Screening> Screenings { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Reservation> Reservations { get; }
    DbSet<ReservationSeat> ReservationSeats { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
