using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Persistence.Configurations;

public sealed class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");

        builder.HasKey(seat => seat.Id);

        builder.Property(seat => seat.Row)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(seat => seat.Number)
            .IsRequired();

        builder.HasIndex(seat => new { seat.AuditoriumId, seat.Row, seat.Number })
            .IsUnique();
    }
}
