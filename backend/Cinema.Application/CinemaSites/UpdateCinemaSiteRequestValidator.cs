using FluentValidation;

namespace Cinema.Application.CinemaSites;

public sealed class UpdateCinemaSiteRequestValidator : AbstractValidator<UpdateCinemaSiteRequest>
{
    public UpdateCinemaSiteRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address).NotNull();

        RuleFor(x => x.Address.Street)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Address.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Address.State)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Address.Country)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Address.PostalCode)
            .NotEmpty()
            .MaximumLength(20);
    }
}
