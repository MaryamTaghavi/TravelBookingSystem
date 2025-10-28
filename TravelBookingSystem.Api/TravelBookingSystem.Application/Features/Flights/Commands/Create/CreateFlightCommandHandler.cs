using MediatR;
using Microsoft.Extensions.Logging;
using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Application.Mappings;
using TravelBookingSystem.Domain.Events;
using Microsoft.VisualBasic;
using TravelBookingSystem.Domain.Entities;

namespace TravelBookingSystem.Application.Features.Flights.Commands.Create;

public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, FlightDto>
{
    private readonly IFlightRepository _flightRepository;
    private readonly ILogger<CreateFlightCommandHandler> _logger;
    private readonly IEventStore _eventStore;
    private readonly ICacheService _cacheService;

    public CreateFlightCommandHandler(
        IFlightRepository flightRepository,
        ILogger<CreateFlightCommandHandler> logger,
        IEventStore eventStore,
        ICacheService cacheService)
    {
        _flightRepository = flightRepository;
        _logger = logger;
        _eventStore = eventStore;
        _cacheService = cacheService;
    }

    public async Task<FlightDto> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        // Check if flight number already exists
        var existingFlight = await _flightRepository.GetByFlightNumberAsync(request.FlightNumber);
        if (existingFlight != null)
        {
            throw new InvalidOperationException("Flight number already exists");
        }

        var flight = new Flight(
            request.FlightNumber,
            request.Origin,
            request.Destination,
            request.DepartureTime,
            request.ArrivalTime,
            request.AvailableSeats,
            request.Price,
            BitConverter.GetBytes(DateTime.Now.Ticks)
        );

        var createdFlight = await _flightRepository.AddAsync(flight);

        var flightCreatedEvent = new FlightCreatedEvent(
            createdFlight.Id,
            createdFlight.FlightNumber,
            createdFlight.Origin,
            createdFlight.Destination
        );

        await _eventStore.SaveEventAsync(
            createdFlight.Id.ToString(),
            "Flight",
            "FlightCreated",
            flightCreatedEvent,
            "system", // In production, this would come from authentication context
            1
        );

        await _cacheService.RemoveByPatternAsync("flights:");

        _logger.LogInformation("Add new flight cache after creating flight {FlightId}", createdFlight.Id);

        return createdFlight.ToDto();
    }
}