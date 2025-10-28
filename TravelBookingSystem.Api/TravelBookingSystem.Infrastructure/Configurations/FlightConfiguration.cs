using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Domain.Entities;

namespace TravelBookingSystem.Infrastructure.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.FlightNumber)
            .IsRequired(true).HasMaxLength(10);

        builder.Property(p => p.Origin)
           .IsRequired(true).HasMaxLength(50);

        builder.Property(p => p.Destination)
            .IsRequired(true).HasMaxLength(50);

        builder.Property(p => p.DepartureTime)
            .IsRequired(true);

        builder.Property(p => p.AvailableSeats)
            .IsRequired(true);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired(true);

        builder.Property(p => p.CreateDate)
            .IsRequired(true);

        builder.HasIndex(e => e.FlightNumber).IsUnique();

        builder.HasMany(e => e.Bookings)
              .WithOne(e => e.Flight)
              .HasForeignKey(e => e.FlightId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.Property(f => f.RowVersion)
              .IsRowVersion()
              .IsConcurrencyToken();
    }
}