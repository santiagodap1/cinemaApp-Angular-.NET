using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Api.Common;
using Cinema.Application.Auditoriums;
using Cinema.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/auditoriums")]
[Produces("application/json")]
public sealed class AuditoriumsController : ControllerBase
{
    private readonly IAuditoriumService _auditoriumService;

    public AuditoriumsController(IAuditoriumService auditoriumService)
    {
        _auditoriumService = auditoriumService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AuditoriumResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<AuditoriumResponse>>> GetAll(
        [FromQuery] Guid? cinemaSiteId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        if (!Pagination.TryNormalize(page, pageSize, out var normalizedPage, out var normalizedPageSize, out var problem))
        {
            return BadRequest(problem);
        }

        var auditoriums = await _auditoriumService.GetAllAsync(cinemaSiteId, normalizedPage, normalizedPageSize, cancellationToken);
        return Ok(auditoriums);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AuditoriumResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuditoriumResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var auditorium = await _auditoriumService.GetByIdAsync(id, cancellationToken);
        if (auditorium is null)
        {
            return NotFound(ApiProblemDetails.NotFound("Auditorium not found."));
        }

        return Ok(auditorium);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AuditoriumResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuditoriumResponse>> Create(CreateAuditoriumRequest request, CancellationToken cancellationToken)
    {
        var result = await _auditoriumService.CreateAsync(request, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value),
            ServiceResultStatus.NotFound => NotFound(ApiProblemDetails.NotFound(result.Error ?? "Cinema site not found.")),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(AuditoriumResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuditoriumResponse>> Update(Guid id, UpdateAuditoriumRequest request, CancellationToken cancellationToken)
    {
        var result = await _auditoriumService.UpdateAsync(id, request, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => Ok(result.Value),
            ServiceResultStatus.NotFound => NotFound(ApiProblemDetails.NotFound(result.Error ?? "Auditorium not found.")),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _auditoriumService.DeleteAsync(id, cancellationToken);
        if (result.Status == ServiceResultStatus.NotFound)
        {
            return NotFound(ApiProblemDetails.NotFound(result.Error ?? "Auditorium not found."));
        }

        return NoContent();
    }
}
