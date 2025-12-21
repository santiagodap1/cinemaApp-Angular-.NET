using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Cinema.Api.Middleware;

public sealed class ApiKeyMiddleware
{
    private const string HeaderName = "X-Api-Key";
    private readonly RequestDelegate _next;
    private readonly string? _apiKey;
    private readonly string? _adminApiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration["Security:ApiKey"];
        _adminApiKey = configuration["Security:AdminApiKey"];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!IsAdminPath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(HeaderName, out var providedKey))
        {
            await DenyAsync(context);
            return;
        }

        var key = providedKey.ToString();
        if (string.IsNullOrWhiteSpace(key))
        {
            await DenyAsync(context);
            return;
        }

        if (!string.IsNullOrWhiteSpace(_adminApiKey))
        {
            if (key != _adminApiKey)
            {
                await DenyAsync(context);
                return;
            }
        }
        else if (!string.IsNullOrWhiteSpace(_apiKey) && key != _apiKey)
        {
            await DenyAsync(context);
            return;
        }

        await _next(context);
    }

    private static bool IsAdminPath(PathString path)
    {
        return path.StartsWithSegments("/api/admin");
    }

    private static async Task DenyAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsJsonAsync(new
        {
            title = "Unauthorized",
            detail = "Invalid or missing API key.",
            status = StatusCodes.Status401Unauthorized
        });
    }
}
