using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _context;

    public FlightRepository(AppDbContext context) => _context = context;

    public async Task<Flight?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Flights
            .Include(f => f.Bookings)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Flight>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Flights
            .Include(f => f.Bookings)
            .ToListAsync(cancellationToken);
    }

    public async Task<Flight> AddAsync(Flight entity, CancellationToken cancellationToken)
    {
        await _context.Flights.AddAsync(entity, cancellationToken);
        return entity;
    }

    public void Update(Flight entity)
    {
        _context.Flights.Update(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var flight = await _context.Flights.FindAsync(id, cancellationToken);
        if (flight != null)
        {
            _context.Flights.Remove(flight);
        }
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Flights.AnyAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Flight>> GetFlightsByFiltersAsync(string? origin, string? destination, DateTime? date,
                 CancellationToken cancellationToken)
    {
        var query = _context.Flights
            .WhereIf(!string.IsNullOrEmpty(origin), f => f.Origin.Contains(origin))
            .WhereIf(!string.IsNullOrEmpty(destination), f => f.Destination.Contains(destination))
            .WhereIf(date.HasValue, f => f.DepartureTime >= date.Value.Date && f.DepartureTime < date.Value.Date.AddDays(1));

        return await query
            .Include(f => f.Bookings)
            .OrderBy(f => f.DepartureTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<Flight?> GetByFlightNumberAsync(string flightNumber, CancellationToken cancellationToken)
    {
        return await _context.Flights
            .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber, cancellationToken);
    }
}