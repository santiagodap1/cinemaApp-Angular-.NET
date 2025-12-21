using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cinema.Infrastructure.Persistence.Configurations;

public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.HasKey(movie => movie.Id);

        builder.Property(movie => movie.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(movie => movie.DurationMinutes)
            .IsRequired();

        builder.Property(movie => movie.Rating)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(movie => movie.Synopsis)
            .HasMaxLength(2000);

        builder.Property(movie => movie.Director)
            .HasMaxLength(200);

        builder.Property(movie => movie.Cast)
            .HasMaxLength(500);

        builder.Property(movie => movie.Language)
            .HasMaxLength(100);

        builder.Property(movie => movie.Country)
            .HasMaxLength(100);

        builder.Property(movie => movie.ReleaseDate);

        builder.HasMany(movie => movie.Screenings)
            .WithOne(screening => screening.Movie)
            .HasForeignKey(screening => screening.MovieId);
    }
}
