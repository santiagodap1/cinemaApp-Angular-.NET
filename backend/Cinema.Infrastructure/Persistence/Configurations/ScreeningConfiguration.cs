using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Persistence.Configurations;

public sealed class ScreeningConfiguration : IEntityTypeConfiguration<Screening>
{
    public void Configure(EntityTypeBuilder<Screening> builder)
    {
        builder.ToTable("Screenings");

        builder.HasKey(screening => screening.Id);

        builder.Property(screening => screening.StartsAt)
            .IsRequired();

        builder.Property(screening => screening.EndsAt)
            .IsRequired();

        builder.Property(screening => screening.Format)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(screening => new { screening.AuditoriumId, screening.StartsAt });

        builder.HasMany(screening => screening.Reservations)
            .WithOne(reservation => reservation.Screening)
            .HasForeignKey(reservation => reservation.ScreeningId);
    }
}
