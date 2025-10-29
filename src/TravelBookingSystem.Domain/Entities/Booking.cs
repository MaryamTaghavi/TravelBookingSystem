namespace TravelBookingSystem.Domain.Entities;

public class Booking : BaseEntity
{
    /// <summary>
    /// شناسه پرواز
    /// </summary>
    public int FlightId { get; private set; }

    /// <summary>
    /// شناسه مسافر
    /// </summary>
    public int PassengerId { get; private set; }

    /// <summary>
    /// شماره صندلی
    /// </summary>
    public string SeatNumber { get; private set; }

    /// <summary>
    /// زمان درج رزرو
    /// </summary>
    public DateTime BookingDate { get; private set; }

    private Booking()
    {
        SeatNumber = string.Empty;
    }

    public Booking(int flightId, int passengerId, string seatNumber)
    {
        ValidateSeatNumber(seatNumber);

        FlightId = flightId;
        PassengerId = passengerId;
        SeatNumber = seatNumber;
        BookingDate = DateTime.UtcNow;
    }

    #region Navigation Property

    public virtual Passenger Passenger { get; private set; } = null!;
    public virtual Flight Flight { get; private set; } = null!;

    #endregion

    private static void ValidateSeatNumber(string seatNumber)
    {
        if (string.IsNullOrWhiteSpace(seatNumber))
            throw new ArgumentException("Seat number is required", nameof(seatNumber));

        if (seatNumber.Length > 10)
            throw new ArgumentException("Seat number cannot exceed 10 characters", nameof(seatNumber));
    }
}
