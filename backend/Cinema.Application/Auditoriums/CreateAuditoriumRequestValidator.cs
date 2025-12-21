using FluentValidation;

namespace Cinema.Application.Auditoriums;

public sealed class CreateAuditoriumRequestValidator : AbstractValidator<CreateAuditoriumRequest>
{
    public CreateAuditoriumRequestValidator()
    {
        RuleFor(x => x.CinemaSiteId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .LessThanOrEqualTo(1000);
    }
}
