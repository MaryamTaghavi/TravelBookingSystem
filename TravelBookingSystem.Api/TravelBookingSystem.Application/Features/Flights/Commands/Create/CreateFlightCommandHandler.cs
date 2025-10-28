using MediatR;
using Microsoft.Extensions.Logging;
using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Application.Mappings;

namespace TravelBookingSystem.Application.Features.Flights.Commands.Create;

public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, FlightDto>
{
    private readonly IFlightRepository _flightRepository;
    private readonly ILogger<CreateFlightCommandHandler> _logger;

    public CreateFlightCommandHandler(
        IFlightRepository flightRepository,
        ILogger<CreateFlightCommandHandler> logger)
    {
        _flightRepository = flightRepository;
        _logger = logger;
    }

    public async Task<FlightDto> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        // Check if flight number already exists
        var existingFlight = await _flightRepository.GetByFlightNumberAsync(request.FlightNumber);
        if (existingFlight != null)
        {
            throw new InvalidOperationException("Flight number already exists");
        }

        var flight = new Domain.Entities.Flight(
            request.FlightNumber,
            request.Origin,
            request.Destination,
            request.DepartureTime,
            request.ArrivalTime,
            request.AvailableSeats,
            request.Price
        );

        var createdFlight = await _flightRepository.AddAsync(flight);
      
        _logger.LogInformation("Invalidated flight cache after creating flight {FlightId}", createdFlight.Id);

        return createdFlight.ToDto();
    }
}