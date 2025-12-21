using System;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.SeatMaps;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/screenings/{screeningId:guid}/seats")]
[Produces("application/json")]
public sealed class SeatMapsController : ControllerBase
{
    private readonly ISeatMapService _seatMapService;

    public SeatMapsController(ISeatMapService seatMapService)
    {
        _seatMapService = seatMapService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SeatMapResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SeatMapResponse>> Get(Guid screeningId, CancellationToken cancellationToken)
    {
        var seatMap = await _seatMapService.GetByScreeningAsync(screeningId, cancellationToken);
        if (seatMap is null)
        {
            return NotFound();
        }

        return Ok(seatMap);
    }
}
