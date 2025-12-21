using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Api.Common;
using Cinema.Application.CinemaSites;
using Cinema.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/cinema-sites")]
[Produces("application/json")]
public sealed class CinemaSitesController : ControllerBase
{
    private readonly ICinemaSiteService _cinemaSiteService;

    public CinemaSitesController(ICinemaSiteService cinemaSiteService)
    {
        _cinemaSiteService = cinemaSiteService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CinemaSiteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<CinemaSiteResponse>>> GetAll(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        if (!Pagination.TryNormalize(page, pageSize, out var normalizedPage, out var normalizedPageSize, out var problem))
        {
            return BadRequest(problem);
        }

        var sites = await _cinemaSiteService.GetAllAsync(normalizedPage, normalizedPageSize, cancellationToken);
        return Ok(sites);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CinemaSiteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CinemaSiteResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var site = await _cinemaSiteService.GetByIdAsync(id, cancellationToken);
        if (site is null)
        {
            return NotFound(ApiProblemDetails.NotFound("Cinema site not found."));
        }

        return Ok(site);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CinemaSiteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CinemaSiteResponse>> Create(CreateCinemaSiteRequest request, CancellationToken cancellationToken)
    {
        var result = await _cinemaSiteService.CreateAsync(request, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CinemaSiteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CinemaSiteResponse>> Update(Guid id, UpdateCinemaSiteRequest request, CancellationToken cancellationToken)
    {
        var result = await _cinemaSiteService.UpdateAsync(id, request, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => Ok(result.Value),
            ServiceResultStatus.NotFound => NotFound(ApiProblemDetails.NotFound(result.Error ?? "Cinema site not found.")),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _cinemaSiteService.DeleteAsync(id, cancellationToken);
        if (result.Status == ServiceResultStatus.NotFound)
        {
            return NotFound(ApiProblemDetails.NotFound(result.Error ?? "Cinema site not found."));
        }

        return NoContent();
    }
}
