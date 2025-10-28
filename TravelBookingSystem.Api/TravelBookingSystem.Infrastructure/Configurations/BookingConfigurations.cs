using TravelBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelBookingSystem.Infrastructure.Configuration;

public class BookingConfigurations : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasOne(b => b.Passenger)
               .WithMany(p => p.Bookings)
               .HasForeignKey(b => b.PassengerId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Flight)
               .WithMany(p => p.Bookings)
               .HasForeignKey(b => b.FlightId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.PassengerId)
                .IsRequired(true);

        builder.Property(p => p.BookingDate)
                .IsRequired(true);

        builder.Property(p => p.SeatNumber)
                .IsRequired(true).HasMaxLength(3);

        builder.Property(p => p.FlightId)
                .IsRequired(true);
    }
}
