using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Domain.Entities;

namespace TravelBookingSystem.Infrastructure.Configurations;

public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.FullName)
            .IsRequired(true).HasMaxLength(50);

        builder.Property(p => p.PassportNumber)
            .IsRequired(true).HasMaxLength(9);

        builder.Property(p => p.Email)
            .IsRequired(true).HasMaxLength(256);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(15)
            .IsRequired(false);

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.PassportNumber).IsUnique();

        builder.HasMany(e => e.Bookings)
              .WithOne(e => e.Passenger)
              .HasForeignKey(e => e.PassengerId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}