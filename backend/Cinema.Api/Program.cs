using Cinema.Application;
using FluentValidation;
using FluentValidation.AspNetCore;
using Cinema.Api.Middleware;
using Cinema.Infrastructure.Background;
using Cinema.Application.Abstractions.Pricing;
using Cinema.Application.Pricing;
using Cinema.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.Configure<PricingOptions>(builder.Configuration.GetSection("Pricing"));
builder.Services.AddSingleton<IPriceCalculator>(sp =>
    new PriceCalculator(sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<PricingOptions>>().Value));
builder.Services.AddValidatorsFromAssemblyContaining<Cinema.Application.Movies.CreateMovieRequestValidator>();
builder.Services.AddHostedService<ReservationExpirationService>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await using var scope = app.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<Cinema.Infrastructure.Persistence.ApplicationDbContext>();
    await Cinema.Infrastructure.Persistence.DatabaseInitializer.SeedAsync(dbContext);
}

app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();

app.Run();

public partial class Program
{
}
