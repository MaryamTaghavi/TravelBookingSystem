using TravelBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flight.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<TravelBookingSystem.Domain.Entities.Flight> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TravelBookingSystem.Domain.Entities.Flight>()
            .Property(f => f.RowVersion)
            .IsRowVersion();
    }

    // TODO : Seed Data for passenger
}