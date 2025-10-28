using System.ComponentModel.DataAnnotations;

namespace TravelBookingSystem.Domain.Entities;

public class Flight : BaseEntity
{
    /// <summary>
    /// شماره پرواز
    /// </summary>
    public string FlightNumber { get; set; }

    /// <summary>
    /// مبدا
    /// </summary>
    public string Origin { get; set; }

    /// <summary>
    /// مقصد
    /// </summary>
    public string Destination { get; set; }

    /// <summary>
    /// زمان حرکت
    /// </summary>
    public DateTime DepartureTime { get; set; }

    /// <summary>
    /// زمان رسیدن
    /// </summary>
    public DateTime ArrivalTime { get; set; }

    /// <summary>
    /// تعداد صندلی های خالی
    /// </summary>
    public int AvailableSeats { get; set; }

    /// <summary>
    /// قیمت بلیط
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// برای کنترل همزمانی
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; }

    #region Navigation Property

    public ICollection<Booking> Bookings { get; set; }

    #endregion
}
