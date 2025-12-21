using System;
using Cinema.Application.Pricing;
using Xunit;

namespace Cinema.Tests;

public sealed class PricingTests
{
    [Fact]
    public void CalculateSeatPrice_Applies_Weekend_And_Evening_Multipliers()
    {
        var options = new PricingOptions
        {
            BasePrice = 10m,
            WeekendMultiplier = 1.2m,
            EveningMultiplier = 1.1m,
            EveningStartHour = 18
        };

        var calculator = new PriceCalculator(options);
        var saturdayEvening = new DateTimeOffset(2025, 1, 4, 20, 0, 0, TimeSpan.Zero);

        var price = calculator.CalculateSeatPrice(saturdayEvening);

        Assert.Equal(13.20m, price);
    }
}
