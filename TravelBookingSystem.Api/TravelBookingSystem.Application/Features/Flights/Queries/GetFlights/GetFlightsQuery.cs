using MediatR;
using TravelBookingSystem.Application.DTOs;

namespace TravelBookingSystem.Application.Features.Flights.Queries.GetFlights;

public class GetFlightsQuery : IRequest<IEnumerable<FlightDto>>
{
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public DateTime? Date { get; set; }
}