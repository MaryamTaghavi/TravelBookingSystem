using TravelBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TravelBookingSystem.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<Event> Events { get; set; }

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