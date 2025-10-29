using FluentValidation;

namespace TravelBookingSystem.Application.Features.Flights.Commands.Create;

public class CreateFlightValidator : AbstractValidator<CreateFlightCommand>
{
    public CreateFlightValidator()
    {
        RuleFor(x => x.FlightNumber)
            .NotEmpty().WithMessage("Flight number is required")
            .MaximumLength(20).WithMessage("Flight number cannot exceed 20 characters");

        RuleFor(x => x.Origin)
            .NotEmpty().WithMessage("Origin is required")
            .MaximumLength(100).WithMessage("Origin cannot exceed 100 characters");

        RuleFor(x => x.Destination)
            .NotEmpty().WithMessage("Destination is required")
            .MaximumLength(100).WithMessage("Destination cannot exceed 100 characters");

        RuleFor(x => x.DepartureTime)
            .NotEmpty().WithMessage("Departure time is required")
            .Must(BeInFuture).WithMessage("Departure time must be in the future");

        RuleFor(x => x.ArrivalTime)
            .NotEmpty().WithMessage("Arrival time is required")
            .GreaterThan(x => x.DepartureTime).WithMessage("Arrival time must be after departure time");

        RuleFor(x => x.AvailableSeats)
            .GreaterThanOrEqualTo(0).WithMessage("Available seats must be non-negative");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }

    private static bool BeInFuture(DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }
}