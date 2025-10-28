using MediatR;
using TravelBookingSystem.Application.DTOs;

namespace TravelBookingSystem.Application.Features.Bookings.Create;

public class CreateBookingCommand : IRequest<BookingResponseDto>
{
    public int FlightId { get; set; }
    public int PassengerId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
}