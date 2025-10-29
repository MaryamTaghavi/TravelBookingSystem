using FluentValidation;

namespace TravelBookingSystem.Application.Features.Flights.Commands.Update;

public class UpdateFlightSeatsValidator : AbstractValidator<UpdateFlightSeatsCommand>
{
    public UpdateFlightSeatsValidator()
    {
        RuleFor(x => x.FlightId)
            .NotEmpty().WithMessage("FlightId is required");

        RuleFor(x => x.AvailableSeats)
            .NotEmpty().WithMessage("AvailableSeats is required");
    }
}
