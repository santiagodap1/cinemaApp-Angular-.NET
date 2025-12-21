using System;
using Cinema.Application.Abstractions.Pricing;

namespace Cinema.Application.Pricing;

public sealed class PriceCalculator : IPriceCalculator
{
    private readonly PricingOptions _options;

    public PriceCalculator(PricingOptions options)
    {
        _options = options;
    }

    public decimal CalculateSeatPrice(DateTimeOffset startsAt)
    {
        var price = _options.BasePrice;

        if (IsWeekend(startsAt))
        {
            price *= _options.WeekendMultiplier;
        }

        if (IsEvening(startsAt))
        {
            price *= _options.EveningMultiplier;
        }

        return decimal.Round(price, 2, MidpointRounding.AwayFromZero);
    }

    private bool IsWeekend(DateTimeOffset startsAt)
    {
        var day = startsAt.DayOfWeek;
        return day == DayOfWeek.Friday || day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;
    }

    private bool IsEvening(DateTimeOffset startsAt)
    {
        return startsAt.Hour >= _options.EveningStartHour;
    }
}
