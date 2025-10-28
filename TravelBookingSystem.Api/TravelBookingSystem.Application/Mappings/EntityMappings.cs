using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Domain.Entities;

namespace TravelBookingSystem.Application.Mappings;

public static class FlightMappings
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

    public static IEnumerable<FlightDto> ToDto(this IEnumerable<Flight> flights)
    {
        return flights.Select(f => f.ToDto());
    }
}

public static class BookingMappings
{
    public static BookingResponseDto ToResponseDto(this Booking booking, Flight flight, Passenger passenger)
    {
        return new BookingResponseDto
        {
            Id = booking.Id,
            FlightNumber = flight.FlightNumber,
            PassengerName = passenger.FullName,
            SeatNumber = booking.SeatNumber,
            BookingDate = booking.BookingDate,
        };
    }

    public static BookingDto ToDto(this Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            FlightId = booking.FlightId,
            PassengerId = booking.PassengerId,
            BookingDate = booking.BookingDate,
            SeatNumber = booking.SeatNumber,
            Flight = booking.Flight?.ToDto(),
            // TODO : Add List Passengers
        };
    }

    public static IEnumerable<BookingDto> ToDto(this IEnumerable<Booking> bookings)
    {
        return bookings.Select(b => b.ToDto());
    }
}
