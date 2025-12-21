using System;

namespace Cinema.Application.Abstractions.Pricing;

public interface IPriceCalculator
{
    decimal CalculateSeatPrice(DateTimeOffset startsAt);
}
