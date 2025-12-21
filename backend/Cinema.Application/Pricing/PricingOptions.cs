namespace Cinema.Application.Pricing;

public sealed class PricingOptions
{
    public decimal BasePrice { get; init; } = 7.50m;
    public decimal WeekendMultiplier { get; init; } = 1.20m;
    public decimal EveningMultiplier { get; init; } = 1.15m;
    public int EveningStartHour { get; init; } = 18;
}
