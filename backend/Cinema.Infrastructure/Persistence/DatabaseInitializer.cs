using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Domain.Entities;
using Cinema.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        if (await dbContext.CinemaSites.AnyAsync())
        {
            return;
        }

        var downtown = new CinemaSite(
            "Downtown Cinema",
            new Address("123 Main St", "Metropolis", "State", "Country", "00000"));

        var uptown = new CinemaSite(
            "Uptown Cinema",
            new Address("450 North Ave", "Metropolis", "State", "Country", "00001"));

        var room1 = CreateAuditorium(downtown, "Room 1", rows: 6, seatsPerRow: 10);
        var room2 = CreateAuditorium(downtown, "Room 2", rows: 5, seatsPerRow: 8);
        var premium = CreateAuditorium(uptown, "Premium", rows: 4, seatsPerRow: 6);

        var movie1 = new Movie(
            "The First Movie",
            120,
            "PG-13",
            "A demo feature.",
            "Alex Rivera",
            "Taylor Smith, Jordan Lee",
            "English",
            "USA",
            new DateOnly(2024, 6, 1));

        var movie2 = new Movie(
            "The Second Movie",
            95,
            "PG",
            "Another demo feature.",
            "Chris Nolan",
            "Morgan Reed, Casey Park",
            "English",
            "USA",
            new DateOnly(2023, 11, 10));

        var movie3 = new Movie(
            "Galaxy Run",
            105,
            "PG-13",
            "A sci-fi adventure across the stars.",
            "Riley Chen",
            "Jamie Fox, Sam Patel",
            "English",
            "UK",
            new DateOnly(2025, 2, 14));

        var baseTime = DateTimeOffset.UtcNow.Date.AddDays(1).AddHours(17);

        var screening1 = new Screening(movie1.Id, room1.Id, baseTime, baseTime.AddMinutes(movie1.DurationMinutes), MovieFormat.Format2D);
        var screening2 = new Screening(movie1.Id, room2.Id, baseTime.AddHours(1), baseTime.AddHours(1).AddMinutes(movie1.DurationMinutes), MovieFormat.Format3D);
        var screening3 = new Screening(movie2.Id, room1.Id, baseTime.AddHours(3), baseTime.AddHours(3).AddMinutes(movie2.DurationMinutes), MovieFormat.Imax);
        var screening4 = new Screening(movie3.Id, premium.Id, baseTime.AddHours(2), baseTime.AddHours(2).AddMinutes(movie3.DurationMinutes), MovieFormat.Imax);

        var customer1 = new Customer("Demo User", "demo@example.com");
        var customer2 = new Customer("Alex Johnson", "alex@example.com");

        var reservation1 = new Reservation(screening1.Id, customer1.Id, 20m, DateTimeOffset.UtcNow);
        var reservation2 = new Reservation(screening1.Id, customer2.Id, 10m, DateTimeOffset.UtcNow);
        reservation1.Confirm();
        reservation2.Confirm();

        var reservedSeats = room1.Seats.Take(3).ToList();
        reservation1.AddSeat(new ReservationSeat(reservation1.Id, reservedSeats[0].Id));
        reservation1.AddSeat(new ReservationSeat(reservation1.Id, reservedSeats[1].Id));
        reservation2.AddSeat(new ReservationSeat(reservation2.Id, reservedSeats[2].Id));

        dbContext.CinemaSites.AddRange(downtown, uptown);
        dbContext.Auditoriums.AddRange(room1, room2, premium);
        dbContext.Seats.AddRange(room1.Seats);
        dbContext.Seats.AddRange(room2.Seats);
        dbContext.Seats.AddRange(premium.Seats);
        dbContext.Movies.AddRange(movie1, movie2, movie3);
        dbContext.Screenings.AddRange(screening1, screening2, screening3, screening4);
        dbContext.Customers.AddRange(customer1, customer2);
        dbContext.Reservations.AddRange(reservation1, reservation2);

        await dbContext.SaveChangesAsync();
    }

    private static Auditorium CreateAuditorium(CinemaSite site, string name, int rows, int seatsPerRow)
    {
        var capacity = rows * seatsPerRow;
        var auditorium = new Auditorium(site.Id, name, capacity);

        for (var rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            var rowLabel = ((char)('A' + rowIndex)).ToString();
            for (var seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
            {
                auditorium.AddSeat(new Seat(auditorium.Id, rowLabel, seatNumber));
            }
        }

        site.AddAuditorium(auditorium);
        return auditorium;
    }
}
