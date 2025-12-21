using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Common;

public static class ApiProblemDetails
{
    public static ProblemDetails NotFound(string detail) => new()
    {
        Title = "Not Found",
        Detail = detail,
        Status = StatusCodes.Status404NotFound
    };

    public static ProblemDetails Conflict(string detail) => new()
    {
        Title = "Conflict",
        Detail = detail,
        Status = StatusCodes.Status409Conflict
    };

    public static ProblemDetails Validation(string detail) => new()
    {
        Title = "Validation Error",
        Detail = detail,
        Status = StatusCodes.Status400BadRequest
    };

    public static ProblemDetails Forbidden(string detail) => new()
    {
        Title = "Forbidden",
        Detail = detail,
        Status = StatusCodes.Status403Forbidden
    };
}
