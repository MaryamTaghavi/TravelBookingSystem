using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Domain.Entities;

namespace TravelBookingSystem.Application.Mappings;

public static class EntityMappings
{
    public static FlightDto ToDto(this Flight flight)
    {
        return new FlightDto
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber,
            Origin = flight.Origin,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
            AvailableSeats = flight.AvailableSeats,
            Price = flight.Price,
            CreateDate = flight.CreateDate
        };
    }
}
