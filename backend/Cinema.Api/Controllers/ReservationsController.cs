using System;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Api.Common;
using Cinema.Application.Common;
using Cinema.Application.Reservations;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/reservations")]
[Produces("application/json")]
public sealed class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReservationResponse>> Create(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var result = await _reservationService.CreateAsync(request, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => Ok(result.Value),
            ServiceResultStatus.NotFound => NotFound(ApiProblemDetails.NotFound(result.Error ?? "Resource not found.")),
            ServiceResultStatus.Conflict => Conflict(ApiProblemDetails.Conflict(result.Error ?? "Conflict.")),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }

    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReservationResponse>> Confirm(Guid id, CancellationToken cancellationToken)
    {
        var result = await _reservationService.ConfirmAsync(id, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => Ok(result.Value),
            ServiceResultStatus.NotFound => NotFound(ApiProblemDetails.NotFound(result.Error ?? "Resource not found.")),
            ServiceResultStatus.Conflict => Conflict(ApiProblemDetails.Conflict(result.Error ?? "Conflict.")),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReservationResponse>> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await _reservationService.CancelAsync(id, cancellationToken);
        return result.Status switch
        {
            ServiceResultStatus.Success => Ok(result.Value),
            ServiceResultStatus.NotFound => NotFound(ApiProblemDetails.NotFound(result.Error ?? "Resource not found.")),
            ServiceResultStatus.Conflict => Conflict(ApiProblemDetails.Conflict(result.Error ?? "Conflict.")),
            ServiceResultStatus.Validation => BadRequest(ApiProblemDetails.Validation(result.Error ?? "Validation failed.")),
            _ => BadRequest(ApiProblemDetails.Validation("Invalid request."))
        };
    }
}
