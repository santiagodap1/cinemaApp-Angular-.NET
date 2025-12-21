using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Abstractions.Data;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Application.Movies;

public sealed class MovieService : IMovieService
{
    private readonly IApplicationDbContext _dbContext;

    public MovieService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<MovieResponse>> GetAllAsync(string? title, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Movies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(title))
        {
            var normalized = title.Trim().ToLower();
            query = query.Where(movie => movie.Title.ToLower().Contains(normalized));
        }

        return await query
            .OrderBy(movie => movie.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(movie => new MovieResponse(
                movie.Id,
                movie.Title,
                movie.DurationMinutes,
                movie.Rating,
                movie.Synopsis,
                movie.Director,
                movie.Cast,
                movie.Language,
                movie.Country,
                movie.ReleaseDate))
            .ToListAsync(cancellationToken);
    }

    public async Task<MovieResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Movies
            .AsNoTracking()
            .Where(movie => movie.Id == id)
            .Select(movie => new MovieResponse(
                movie.Id,
                movie.Title,
                movie.DurationMinutes,
                movie.Rating,
                movie.Synopsis,
                movie.Director,
                movie.Cast,
                movie.Language,
                movie.Country,
                movie.ReleaseDate))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<MovieResponse> CreateAsync(CreateMovieRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var movie = new Movie(
            request.Title,
            request.DurationMinutes,
            request.Rating,
            request.Synopsis,
            request.Director,
            request.Cast,
            request.Language,
            request.Country,
            request.ReleaseDate);

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new MovieResponse(
            movie.Id,
            movie.Title,
            movie.DurationMinutes,
            movie.Rating,
            movie.Synopsis,
            movie.Director,
            movie.Cast,
            movie.Language,
            movie.Country,
            movie.ReleaseDate);
    }
}
