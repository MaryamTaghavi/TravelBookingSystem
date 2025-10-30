using System.ComponentModel.DataAnnotations;

namespace TravelBookingSystem.Domain.Entities;

public class Flight : BaseEntity
{
    public string FlightNumber { get; private set; }

    public string Origin { get; private set; }

    public string Destination { get; private set; }

    public DateTime DepartureTime { get; private set; }

    public DateTime ArrivalTime { get; private set; }

    public int AvailableSeats { get; private set; }

    public decimal Price { get; private set; }

    public DateTime CreateDate { get; private set; }

    /// <summary>
    /// Control Concurrency
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>();

    #region Navigation Property

    public ICollection<Booking> Bookings { get; private set; } = new List<Booking>();

    #endregion

    private Flight()
    {
        FlightNumber = string.Empty;
        Origin = string.Empty;
        Destination = string.Empty;
    }

    // Because using In-Memory Database must set value for rowVersion
    public Flight(string flightNumber, string origin, string destination,
                DateTime departureTime, DateTime arrivalTime, int availableSeats, decimal price, byte[] rowVersion)
    {
        ValidateFlightNumber(flightNumber);
        ValidateLocation(origin, nameof(origin));
        ValidateLocation(destination, nameof(destination));
        ValidateTimes(departureTime, arrivalTime);
        ValidateSeats(availableSeats);
        ValidatePrice(price);

        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        AvailableSeats = availableSeats;
        Price = price;
        CreateDate = DateTime.UtcNow;
        RowVersion = rowVersion;
    }

    public void UpdateSeats(int newSeatCount)
    {
        ValidateSeats(newSeatCount);
        AvailableSeats = newSeatCount;
        UpdateDate = DateTime.UtcNow;
    }

    public void ReduceAvailableSeats()
    {
        if (AvailableSeats < 0)
            throw new InvalidOperationException("No seats available for this flight");

        AvailableSeats--;
        UpdateDate = DateTime.UtcNow;
    }

    public bool HasAvailableSeats() => AvailableSeats > 0;

    private static void ValidateFlightNumber(string flightNumber)
    {
        if (string.IsNullOrWhiteSpace(flightNumber))
            throw new ArgumentException("Flight number is required", nameof(flightNumber));

        if (flightNumber.Length > 20)
            throw new ArgumentException("Flight number cannot exceed 20 characters", nameof(flightNumber));
    }

    private static void ValidateLocation(string location, string paramName)
    {
        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException($"{paramName} is required", paramName);

        if (location.Length > 100)
            throw new ArgumentException($"{paramName} cannot exceed 100 characters", paramName);
    }

    private static void ValidateTimes(DateTime departureTime, DateTime arrivalTime)
    {
        if (departureTime >= arrivalTime)
            throw new ArgumentException("Departure time must be before arrival time");

        if (departureTime <= DateTime.UtcNow)
            throw new ArgumentException("Departure time must be in the future");
    }

    private static void ValidateSeats(int seats)
    {
        if (seats < 0)
            throw new ArgumentException("Available seats cannot be negative");
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than 0");
    }
}
