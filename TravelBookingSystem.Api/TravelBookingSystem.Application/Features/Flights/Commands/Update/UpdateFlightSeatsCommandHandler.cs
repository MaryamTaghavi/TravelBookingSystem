using MediatR;
using Microsoft.Extensions.Logging;
using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Domain.Events;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Application.Mappings;

namespace TravelBookingSystem.Application.Features.Flights.Commands.Update;

public class UpdateFlightSeatsCommandHandler : IRequestHandler<UpdateFlightSeatsCommand, FlightDto>
{
    private readonly IFlightRepository _flightRepository;
    private readonly IEventStore _eventStore;
    private readonly ICacheService _cacheService;
    private readonly ILogger<UpdateFlightSeatsCommandHandler> _logger;

    public UpdateFlightSeatsCommandHandler(
        IFlightRepository flightRepository,
        IEventStore eventStore,
        ICacheService cacheService,
        ILogger<UpdateFlightSeatsCommandHandler> logger)
    {
        _flightRepository = flightRepository;
        _eventStore = eventStore;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<FlightDto> Handle(UpdateFlightSeatsCommand request, CancellationToken cancellationToken)
    {
        var flight = await _flightRepository.GetByIdAsync(request.FlightId , cancellationToken);
        if (flight == null)
        {
            throw new KeyNotFoundException("Flight not found");
        }

        var previousSeatCount = flight.AvailableSeats;
        flight.UpdateSeats(request.AvailableSeats);
        await _flightRepository.UpdateAsync(flight, cancellationToken);

        // Store event
        var seatsUpdatedEvent = new FlightSeatsUpdatedEvent(
            flight.Id,
            previousSeatCount,
            request.AvailableSeats
        );

        await _eventStore.SaveEventAsync(
            flight.Id.ToString(),
            "Flight",
            "FlightSeatsUpdated",
            seatsUpdatedEvent,
            "system", // TODO: In production, this would come from authentication context
            2 // TODO: This should be incremented based on existing events
        );

        await _cacheService.RemoveByPatternAsync("flights:");

        _logger.LogInformation("Update flight cache after updating seats for flight {FlightId}", flight.Id);

        return flight.ToDto();
    }
}