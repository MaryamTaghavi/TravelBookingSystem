using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Domain.Entities;

namespace TravelBookingSystem.Infrastructure.Configurations;

public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.Property(p => p.FullName)
            .IsRequired(true).HasMaxLength(50);

        builder.Property(p => p.PassportNumber)
            .IsRequired(true).HasMaxLength(9);

        builder.Property(p => p.Email)
            .IsRequired(true).HasMaxLength(256);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(15)
            .IsRequired(false);
    }
}