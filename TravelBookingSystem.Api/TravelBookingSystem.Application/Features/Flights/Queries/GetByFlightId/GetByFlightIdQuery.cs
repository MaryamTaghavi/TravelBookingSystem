using MediatR;
using TravelBookingSystem.Application.DTOs;

namespace TravelBookingSystem.Application.Features.Flights.Queries.GetByFlightId;

public class GetByFlightIdQuery : IRequest<IEnumerable<BookingDto>>
{
    public int FlightId { get; set; }
}