using MediatR;
using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Application.Mappings;

namespace TravelBookingSystem.Application.Features.Flights.Queries.GetByFlightId;

public class GetBookingsByFlightIdQueryHandler : IRequestHandler<GetByFlightIdQuery, IEnumerable<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingsByFlightIdQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<BookingDto>> Handle(GetByFlightIdQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetBookingsByFlightIdAsync(request.FlightId, cancellationToken);
        return bookings.ToDto();
    }
}
