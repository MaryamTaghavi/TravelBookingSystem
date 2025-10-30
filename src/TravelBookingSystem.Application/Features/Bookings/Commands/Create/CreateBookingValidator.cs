using FluentValidation;

namespace TravelBookingSystem.Application.Features.Bookings.Commands.Create;

public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.FlightId)
            .NotEmpty().WithMessage("FlightId is required")
            .GreaterThan(0).WithMessage("FlightId must greater than 0");

        RuleFor(x => x.PassengerId)
            .NotEmpty().WithMessage("PassengerId is required")
            .GreaterThan(0).WithMessage("PassengerId must greater than 0");

        RuleFor(x => x.SeatNumber)
            .NotEmpty().WithMessage("SeatNumber is required")
            .MaximumLength(5).WithMessage("SeatNumber cannot exceed 5 characters");
    }
}
