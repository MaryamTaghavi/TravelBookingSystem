using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _context;

    public FlightRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Flight?> GetByIdAsync(int id)
    {
        return await _context.Flights
            .Include(f => f.Bookings)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Flight>> GetAllAsync()
    {
        return await _context.Flights
            .Include(f => f.Bookings)
            .ToListAsync();
    }

    public async Task<Flight> AddAsync(Flight entity)
    {
        _context.Flights.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Flight entity)
    {
        _context.Flights.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var flight = await _context.Flights.FindAsync(id);
        if (flight != null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Flights.AnyAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Flight>> GetFlightsByFiltersAsync(string? origin, string? destination, DateTime? date)
    {
        var query = _context.Flights.AsQueryable();

        if (!string.IsNullOrEmpty(origin))
        {
            query = query.Where(f => f.Origin.Contains(origin));
        }

        if (!string.IsNullOrEmpty(destination))
        {
            query = query.Where(f => f.Destination.Contains(destination));
        }

        if (date.HasValue)
        {
            var startOfDay = date.Value.Date;
            var endOfDay = startOfDay.AddDays(1);
            query = query.Where(f => f.DepartureTime >= startOfDay && f.DepartureTime < endOfDay);
        }

        return await query
            .Include(f => f.Bookings)
            .OrderBy(f => f.DepartureTime)
            .ToListAsync();
    }

    public async Task<Flight?> GetByFlightNumberAsync(string flightNumber)
    {
        return await _context.Flights
            .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber);
    }
}