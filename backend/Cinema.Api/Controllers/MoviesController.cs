using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Api.Common;
using Cinema.Application.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/movies")]
[Produces("application/json")]
public sealed class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MovieResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<MovieResponse>>> GetAll(
        [FromQuery] string? title,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        if (!Pagination.TryNormalize(page, pageSize, out var normalizedPage, out var normalizedPageSize, out var problem))
        {
            return BadRequest(problem);
        }

        var movies = await _movieService.GetAllAsync(title, normalizedPage, normalizedPageSize, cancellationToken);
        return Ok(movies);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var movie = await _movieService.GetByIdAsync(id, cancellationToken);
        if (movie is null)
        {
            return NotFound(ApiProblemDetails.NotFound("Movie not found."));
        }

        return Ok(movie);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MovieResponse>> Create(CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = await _movieService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }
}
