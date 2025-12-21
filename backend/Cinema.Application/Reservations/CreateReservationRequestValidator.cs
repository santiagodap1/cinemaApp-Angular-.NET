using System.Linq;
using FluentValidation;

namespace Cinema.Application.Reservations;

public sealed class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationRequestValidator()
    {
        RuleFor(x => x.ScreeningId)
            .NotEmpty();

        RuleFor(x => x.CustomerEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320);

        RuleFor(x => x.CustomerFullName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.SeatIds)
            .NotEmpty();

        RuleForEach(x => x.SeatIds)
            .NotEmpty();

        RuleFor(x => x.SeatIds)
            .Must(seatIds => seatIds.Distinct().Count() == seatIds.Count)
            .WithMessage("SeatIds must be unique.");
    }
}
