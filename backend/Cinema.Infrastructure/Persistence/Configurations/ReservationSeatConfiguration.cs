using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Persistence.Configurations;

public sealed class ReservationSeatConfiguration : IEntityTypeConfiguration<ReservationSeat>
{
    public void Configure(EntityTypeBuilder<ReservationSeat> builder)
    {
        builder.ToTable("ReservationSeats");

        builder.HasKey(reservationSeat => reservationSeat.Id);

        builder.HasIndex(reservationSeat => new { reservationSeat.ReservationId, reservationSeat.SeatId })
            .IsUnique();

        builder.HasOne(reservationSeat => reservationSeat.Seat)
            .WithMany()
            .HasForeignKey(reservationSeat => reservationSeat.SeatId);
    }
}
