namespace TravelBookingSystem.Domain.Entities;

public class Passenger : BaseEntity
{
    /// <summary>
    /// نام کامل
    /// </summary>
    public string FullName { get; private set; }

    /// <summary>
    /// ایمیل
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// شماره پاسپورت
    /// </summary>
    public string PassportNumber { get; private set; }

    /// <summary>
    /// شماره تماس
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// زمان ایجاد
    /// </summary>
    public DateTime CreateDate { get; private set; }

    #region Navigation Property

    public ICollection<Booking> Bookings { get; private set; } = new List<Booking>();

    #endregion

    private Passenger()
    {
        FullName = string.Empty;
        Email = string.Empty;
        PassportNumber = string.Empty;
    }

    public Passenger(string fullName, string email, string passportNumber, string? phoneNumber = null)
    {
        ValidateFullName(fullName);
        ValidateEmail(email);
        ValidatePassportNumber(passportNumber);
        ValidatePhoneNumber(phoneNumber);

        FullName = fullName;
        Email = email;
        PassportNumber = passportNumber;
        PhoneNumber = phoneNumber;
        CreateDate = DateTime.UtcNow;
    }

    public void UpdateContactInfo(string? phoneNumber)
    {
        ValidatePhoneNumber(phoneNumber);
        PhoneNumber = phoneNumber;
        UpdateDate = DateTime.UtcNow;
    }

    private static void ValidateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name is required", nameof(fullName));

        if (fullName.Length > 200)
            throw new ArgumentException("Full name cannot exceed 200 characters", nameof(fullName));
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email cannot exceed 255 characters", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));
    }

    private static void ValidatePassportNumber(string passportNumber)
    {
        if (string.IsNullOrWhiteSpace(passportNumber))
            throw new ArgumentException("Passport number is required", nameof(passportNumber));

        if (passportNumber.Length > 50)
            throw new ArgumentException("Passport number cannot exceed 50 characters", nameof(passportNumber));
    }

    private static void ValidatePhoneNumber(string? phoneNumber)
    {
        if (phoneNumber != null && phoneNumber.Length > 20)
            throw new ArgumentException("Phone number cannot exceed 20 characters", nameof(phoneNumber));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
