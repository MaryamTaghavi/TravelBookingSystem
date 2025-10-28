namespace TravelBookingSystem.Application.DTOs;


public class BookingDto
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public int PassengerId { get; set; }
    public DateTime BookingDate { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public FlightDto? Flight { get; set; }
}

public class BookingResponseDto
{
    public int Id { get; set; }
    public string FlightNumber { get; set; } = string.Empty;
    public string PassengerName { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
}