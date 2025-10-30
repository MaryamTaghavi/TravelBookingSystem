using System.ComponentModel.DataAnnotations;

namespace TravelBookingSystem.Domain.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsDeleted { get; set; } = false;
}
