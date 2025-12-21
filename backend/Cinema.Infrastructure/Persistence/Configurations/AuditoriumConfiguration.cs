using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Persistence.Configurations;

public sealed class AuditoriumConfiguration : IEntityTypeConfiguration<Auditorium>
{
    public void Configure(EntityTypeBuilder<Auditorium> builder)
    {
        builder.ToTable("Auditoriums");

        builder.HasKey(auditorium => auditorium.Id);

        builder.Property(auditorium => auditorium.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(auditorium => auditorium.Capacity)
            .IsRequired();

        builder.HasMany(auditorium => auditorium.Seats)
            .WithOne(seat => seat.Auditorium)
            .HasForeignKey(seat => seat.AuditoriumId);
    }
}
