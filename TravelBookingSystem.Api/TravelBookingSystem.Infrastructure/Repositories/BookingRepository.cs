using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context) => _context = context;
    public async Task<Booking?> GetByIdAsync(int id)
    {
        return await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .ToListAsync();
    }

    public async Task<Booking> AddAsync(Booking entity)
    {
        _context.Bookings.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Booking entity)
    {
        _context.Bookings.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Bookings.AnyAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId)
    {
        return await _context.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Passenger)
            .Where(b => b.FlightId == flightId)
            .OrderBy(b => b.SeatNumber)
            .ToListAsync();
    }

    public async Task<bool> IsSeatAvailableAsync(int flightId, string seatNumber)
    {
        return !await _context.Bookings
            .AnyAsync(b => b.FlightId == flightId && b.SeatNumber == seatNumber);
    }
}