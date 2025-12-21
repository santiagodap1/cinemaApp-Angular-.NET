using FluentValidation;

namespace Cinema.Application.Auditoriums;

public sealed class UpdateAuditoriumRequestValidator : AbstractValidator<UpdateAuditoriumRequest>
{
    public UpdateAuditoriumRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000);
    }
}
