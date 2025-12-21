using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Api.Common;
using Cinema.Application.Common;
using Cinema.Application.Seats;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/auditoriums/{auditoriumId:guid}/seats")]
[Produces("application/json")]
public sealed class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SeatResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<SeatResponse>>> GetAll(
        Guid auditoriumId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        if (!Pagination.TryNormalize(page, pageSize, out var normalizedPage, out var normalizedPageSize, out var problem))
        {
            return BadRequest(problem);
        }

        var seats = await _seatService.GetByAuditoriumAsync(auditoriumId, normalizedPage, normalizedPageSize, cancellationToken);
        return Ok(seats);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SeatResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<SeatResponse>> Create(Guid auditoriumId, CreateSeatRequest request, CancellationToken cancellationToken)
    {
        if (auditoriumId != request.AuditoriumId)
        {
            return BadRequest(ApiProblemDetails.Validation("AuditoriumId mismatch."));
        }

        var result = await _seatService.CreateAsync(request, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => CreatedAtAction(nameof(GetAll), new { auditoriumId }, result.Value),
            ServiceResultStatus.NotFound => NotFound(ApiProblemDetails.NotFound(result.Error ?? "Auditorium not found.")),
            ServiceResultStatus.Conflict => Conflict(ApiProblemDetails.Conflict(result.Error ?? "Seat already exists.")),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }

    [HttpDelete("{seatId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid auditoriumId, Guid seatId, CancellationToken cancellationToken)
    {
        var result = await _seatService.DeleteAsync(seatId, cancellationToken);
        if (result.Status == ServiceResultStatus.NotFound)
        {
            return NotFound(ApiProblemDetails.NotFound(result.Error ?? "Seat not found."));
        }

        return NoContent();
    }
}
