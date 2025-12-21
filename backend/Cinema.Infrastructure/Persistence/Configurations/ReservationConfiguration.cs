using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Persistence.Configurations;

public sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(reservation => reservation.Id);

        builder.Property(reservation => reservation.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(reservation => reservation.TotalAmount)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(reservation => reservation.ReservedAt)
            .IsRequired();

        builder.HasMany(reservation => reservation.Seats)
            .WithOne(seat => seat.Reservation)
            .HasForeignKey(seat => seat.ReservationId);
    }
}
