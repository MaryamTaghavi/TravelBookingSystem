using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _context;

    public FlightRepository(AppDbContext context) => _context = context;

    public async Task<Flight?> GetByIdAsync(int id , CancellationToken cancellationToken)
    {
        return await _context.Flights
            .Include(f => f.Bookings)
            .FirstOrDefaultAsync(f => f.Id == id ,cancellationToken);
    }

    public async Task<IEnumerable<Flight>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Flights
            .Include(f => f.Bookings)
            .ToListAsync(cancellationToken);
    }

    public async Task<Flight> AddAsync(Flight entity , CancellationToken cancellationToken)
    {
        _context.Flights.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(Flight entity, CancellationToken cancellationToken)
    {
        _context.Flights.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var flight = await _context.Flights.FindAsync(id , cancellationToken);
        if (flight != null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Flights.AnyAsync(f => f.Id == id , cancellationToken);
    }

    public async Task<IEnumerable<Flight>> GetFlightsByFiltersAsync(string? origin, string? destination, DateTime? date ,
                 CancellationToken cancellationToken)
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
            .ToListAsync(cancellationToken);
    }

    public async Task<Flight?> GetByFlightNumberAsync(string flightNumber , CancellationToken cancellationToken)
    {
        return await _context.Flights
            .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber , cancellationToken);
    }
}