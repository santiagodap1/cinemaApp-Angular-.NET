using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Common;

public static class Pagination
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    public static bool TryNormalize(int? page, int? pageSize, out int normalizedPage, out int normalizedPageSize, out ProblemDetails? problem)
    {
        normalizedPage = page ?? DefaultPage;
        normalizedPageSize = pageSize ?? DefaultPageSize;
        problem = null;

        if (normalizedPage < 1)
        {
            problem = ApiProblemDetails.Validation("Page must be greater than or equal to 1.");
            return false;
        }

        if (normalizedPageSize < 1 || normalizedPageSize > MaxPageSize)
        {
            problem = ApiProblemDetails.Validation($"PageSize must be between 1 and {MaxPageSize}.");
            return false;
        }

        return true;
    }
}
