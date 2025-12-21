using System.Threading;
using System.Threading.Tasks;
using Cinema.Api.Common;
using Cinema.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/admin")]
public sealed class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHostEnvironment _environment;

    public AdminController(ApplicationDbContext dbContext, IHostEnvironment environment)
    {
        _dbContext = dbContext;
        _environment = environment;
    }

    [HttpPost("seed")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Seed(CancellationToken cancellationToken)
    {
        if (!_environment.IsDevelopment())
        {
            return StatusCode(StatusCodes.Status403Forbidden, ApiProblemDetails.Forbidden("Seeding is only allowed in development."));
        }

        await DatabaseInitializer.SeedAsync(_dbContext);
        return Ok(new { status = "seeded" });
    }

    [HttpGet("validate")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public IActionResult Validate()
    {
        return Ok(new { status = "ok" });
    }
}
