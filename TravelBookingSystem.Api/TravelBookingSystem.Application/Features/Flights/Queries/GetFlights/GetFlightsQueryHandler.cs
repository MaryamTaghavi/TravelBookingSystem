using MediatR;
using Microsoft.Extensions.Logging;
using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Application.Mappings;

namespace TravelBookingSystem.Application.Features.Flights.Queries.GetFlights;

public class GetFlightsQueryHandler : IRequestHandler<GetFlightsQuery, IEnumerable<FlightDto>>
{
    private readonly IFlightRepository _flightRepository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetFlightsQueryHandler> _logger;

    public GetFlightsQueryHandler(
        IFlightRepository flightRepository,
        ICacheService cacheService,
        ILogger<GetFlightsQueryHandler> logger)
    {
        _flightRepository = flightRepository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<IEnumerable<FlightDto>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        // Create cache key based on query parameters
        var cacheKey = CreateCacheKey(request);

        // Try to get from cache first
        var cachedFlights = await _cacheService.GetAsync<IEnumerable<FlightDto>>(cacheKey);
        if (cachedFlights != null)
        {
            _logger.LogInformation("Retrieved flights from cache for key: {CacheKey}", cacheKey);
            return cachedFlights;
        }

        // If not in cache, get from database
        var flights = await _flightRepository.GetFlightsByFiltersAsync(
            request.Origin,
            request.Destination,
            request.Date);

        var flightDtos = flights.ToDto();

        // Cache the results for 15 minutes
        await _cacheService.SetAsync(cacheKey, flightDtos, TimeSpan.FromMinutes(15));

        _logger.LogInformation("Cached flights for key: {CacheKey}", cacheKey);

        return flightDtos;
    }

    private static string CreateCacheKey(GetFlightsQuery request)
    {
        var keyParts = new List<string> { "flights" };

        if (!string.IsNullOrEmpty(request.Origin))
            keyParts.Add($"origin:{request.Origin.ToLower()}");

        if (!string.IsNullOrEmpty(request.Destination))
            keyParts.Add($"destination:{request.Destination.ToLower()}");

        if (request.Date.HasValue)
            keyParts.Add($"date:{request.Date.Value:yyyy-MM-dd}");

        return string.Join(":", keyParts);
    }
}