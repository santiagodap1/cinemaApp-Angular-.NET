using FluentValidation;

namespace Cinema.Application.Movies;

public sealed class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0)
            .LessThanOrEqualTo(400);

        RuleFor(x => x.Rating)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Synopsis)
            .MaximumLength(2000);

        RuleFor(x => x.Director)
            .MaximumLength(200);

        RuleFor(x => x.Cast)
            .MaximumLength(500);

        RuleFor(x => x.Language)
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .MaximumLength(100);
    }
}
