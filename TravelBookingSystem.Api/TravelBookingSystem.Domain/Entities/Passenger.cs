namespace TravelBookingSystem.Domain.Entities;

public class Passenger : BaseEntity
{
    /// <summary>
    /// نام کامل
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// ایمیل
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// شماره پاسپورت
    /// </summary>
    public string PassportNumber { get; set; }

    /// <summary>
    /// شماره تماس
    /// </summary>
    public string? PhoneNumber { get; set; }

    #region Navigation Property

    public ICollection<Booking> Bookings { get; set; }

    #endregion
}
