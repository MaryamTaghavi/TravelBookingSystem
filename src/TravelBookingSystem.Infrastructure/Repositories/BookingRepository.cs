using Microsoft.EntityFrameworkCore;
using System.Threading;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context) => _context = context;
    public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .FirstOrDefaultAsync(b => b.Id == id , cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .ToListAsync(cancellationToken);
    }

    public async Task<Booking> AddAsync(Booking entity , CancellationToken cancellationToken = default)
    {
        await _context.Bookings.AddAsync(entity , cancellationToken);
        return entity;
    }

    public void Update(Booking entity)
    {
        _context.Bookings.Update(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings.FindAsync(id , cancellationToken);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
        }
    }

    public async Task<bool> ExistsAsync(int id , CancellationToken cancellationToken)
    {
        return await _context.Bookings.AnyAsync(b => b.Id == id , cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId, CancellationToken cancellationToken)
    {
        return await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .Where(b => b.FlightId == flightId)
            .OrderBy(b => b.SeatNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsSeatAvailableAsync(int flightId, string seatNumber, CancellationToken cancellationToken)
    {
        return !await _context.Bookings
            .AnyAsync(b => b.FlightId == flightId && b.SeatNumber == seatNumber , cancellationToken);
    }
}