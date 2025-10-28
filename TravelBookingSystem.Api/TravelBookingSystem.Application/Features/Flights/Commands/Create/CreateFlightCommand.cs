using TravelBookingSystem.Application.DTOs;
using MediatR;

namespace TravelBookingSystem.Application.Features.Flights.Commands.Create;

public class CreateFlightCommand : IRequest<FlightDto>
{
    public string FlightNumber { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public int AvailableSeats { get; set; }
    public decimal Price { get; set; }
}