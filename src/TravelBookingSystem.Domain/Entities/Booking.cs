using System.ComponentModel.DataAnnotations.Schema;

namespace TravelBookingSystem.Domain.Entities;

public class Booking : BaseEntity
{
    public int FlightId { get; private set; }

    public int PassengerId { get; private set; }

    public string SeatNumber { get; private set; }

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

    [ForeignKey(nameof(PassengerId))]
    public virtual Passenger Passenger { get; private set; } = null!;

    [ForeignKey(nameof(FlightId))]
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
