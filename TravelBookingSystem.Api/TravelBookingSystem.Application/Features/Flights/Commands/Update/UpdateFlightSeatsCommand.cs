using MediatR;
using TravelBookingSystem.Application.DTOs;

namespace TravelBookingSystem.Application.Features.Flights.Commands.Update;

public class UpdateFlightSeatsCommand : IRequest<FlightDto>
{
    public int FlightId { get; set; }
    public int AvailableSeats { get; set; }
}