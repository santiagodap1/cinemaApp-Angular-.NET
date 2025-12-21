using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinema.Application.Abstractions.Data;
using Cinema.Application.Abstractions.Time;
using Cinema.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cinema.Infrastructure.Background;

public sealed class ReservationExpirationService : BackgroundService
{
    private static readonly TimeSpan HoldDuration = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;

    public ReservationExpirationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ExpireReservationsAsync(stoppingToken);
            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task ExpireReservationsAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var timeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

        var threshold = timeProvider.UtcNow.Subtract(HoldDuration);

        var expired = await dbContext.Reservations
            .Where(r => r.Status == ReservationStatus.Pending && r.ReservedAt <= threshold)
            .ToListAsync(cancellationToken);

        if (expired.Count == 0)
        {
            return;
        }

        foreach (var reservation in expired)
        {
            reservation.Expire();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
