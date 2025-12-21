using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Api.Common;
using Cinema.Application.Common;
using Cinema.Application.Screenings;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/screenings")]
[Produces("application/json")]
public sealed class ScreeningsController : ControllerBase
{
    private readonly IScreeningService _screeningService;

    public ScreeningsController(IScreeningService screeningService)
    {
        _screeningService = screeningService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ScreeningResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<ScreeningResponse>>> GetAll(
        [FromQuery] Guid? movieId,
        [FromQuery] Guid? cinemaSiteId,
        [FromQuery] DateOnly? date,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        if (!Pagination.TryNormalize(page, pageSize, out var normalizedPage, out var normalizedPageSize, out var problem))
        {
            return BadRequest(problem);
        }

        var screenings = await _screeningService.GetAllAsync(movieId, cinemaSiteId, date, normalizedPage, normalizedPageSize, cancellationToken);
        return Ok(screenings);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ScreeningResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScreeningResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var screening = await _screeningService.GetByIdAsync(id, cancellationToken);
        if (screening is null)
        {
            return NotFound(ApiProblemDetails.NotFound("Screening not found."));
        }

        return Ok(screening);
    }

    [HttpGet("{id:guid}/occupancy")]
    [ProducesResponseType(typeof(ScreeningOccupancyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScreeningOccupancyResponse>> GetOccupancy(Guid id, CancellationToken cancellationToken)
    {
        var occupancy = await _screeningService.GetOccupancyAsync(id, cancellationToken);
        if (occupancy is null)
        {
            return NotFound(ApiProblemDetails.NotFound("Screening not found."));
        }

        return Ok(occupancy);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ScreeningResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ScreeningResponse>> Create(CreateScreeningRequest request, CancellationToken cancellationToken)
    {
        var result = await _screeningService.CreateAsync(request, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value),
            ServiceResultStatus.NotFound => NotFound(ApiProblemDetails.NotFound(result.Error ?? "Resource not found.")),
            ServiceResultStatus.Conflict => Conflict(ApiProblemDetails.Conflict(result.Error ?? "Conflict.")),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }
}
