using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Persistence.Configurations;

public sealed class CinemaSiteConfiguration : IEntityTypeConfiguration<CinemaSite>
{
    public void Configure(EntityTypeBuilder<CinemaSite> builder)
    {
        builder.ToTable("CinemaSites");

        builder.HasKey(site => site.Id);

        builder.Property(site => site.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(site => site.Address, address =>
        {
            address.Property(a => a.Street).HasMaxLength(200).IsRequired();
            address.Property(a => a.City).HasMaxLength(100).IsRequired();
            address.Property(a => a.State).HasMaxLength(100).IsRequired();
            address.Property(a => a.Country).HasMaxLength(100).IsRequired();
            address.Property(a => a.PostalCode).HasMaxLength(20).IsRequired();
        });

        builder.HasMany(site => site.Auditoriums)
            .WithOne(auditorium => auditorium.CinemaSite)
            .HasForeignKey(auditorium => auditorium.CinemaSiteId);
    }
}
