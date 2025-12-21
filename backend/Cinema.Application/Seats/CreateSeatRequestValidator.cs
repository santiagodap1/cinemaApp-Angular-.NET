using FluentValidation;

namespace Cinema.Application.Seats;

public sealed class CreateSeatRequestValidator : AbstractValidator<CreateSeatRequest>
{
    public CreateSeatRequestValidator()
    {
        RuleFor(x => x.AuditoriumId)
            .NotEmpty();

        RuleFor(x => x.Row)
            .NotEmpty()
            .MaximumLength(5);

        RuleFor(x => x.Number)
            .GreaterThan(0)
            .LessThanOrEqualTo(500);
    }
}
