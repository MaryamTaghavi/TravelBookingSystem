using TravelBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Infrastructure.Configurations;

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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlightConfiguration).Assembly);
    }

    // TODO : Seed Data for passenger
}