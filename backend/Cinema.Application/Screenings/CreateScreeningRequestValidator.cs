using FluentValidation;

namespace Cinema.Application.Screenings;

public sealed class CreateScreeningRequestValidator : AbstractValidator<CreateScreeningRequest>
{
    public CreateScreeningRequestValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEmpty();

        RuleFor(x => x.AuditoriumId)
            .NotEmpty();

        RuleFor(x => x.StartsAt)
            .NotEmpty();

        RuleFor(x => x.EndsAt)
            .NotEmpty()
            .GreaterThan(x => x.StartsAt);

        RuleFor(x => x.Format)
            .IsInEnum();
    }
}
