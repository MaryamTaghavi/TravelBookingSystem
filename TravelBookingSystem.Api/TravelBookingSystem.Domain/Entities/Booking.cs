namespace TravelBookingSystem.Domain.Entities;

public class Booking : BaseEntity
{
    /// <summary>
    /// شناسه پرواز
    /// </summary>
    public int FlightId { get; set; }

    /// <summary>
    /// شناسه مسافر
    /// </summary>
    public int PassengerId { get; set; }

    /// <summary>
    /// شماره صندلی
    /// </summary>
    public int SeatNumber { get; set; }

    #region Navigation Property

    public Passenger Passenger { get; set; }
    public Flight Flight { get; set; }

    #endregion
}
