using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cinema.Application.Movies;

public interface IMovieService
{
    Task<IReadOnlyList<MovieResponse>> GetAllAsync(string? title, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<MovieResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<MovieResponse> CreateAsync(CreateMovieRequest request, CancellationToken cancellationToken = default);
}
