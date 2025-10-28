using System.ComponentModel.DataAnnotations;

namespace TravelBookingSystem.Domain.Entities;

//TODO : Reach domain model
public class BaseEntity
{
    /// <summary>
    /// شناسه
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// تاریخ ویرایش
    /// </summary>
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// حذف شده؟
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}
