using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Infrastructure.Repositories;

public class PassengerRepository : IPassengerRepository
{
    private readonly AppDbContext _context;

    public PassengerRepository(AppDbContext context) => _context = context;

    public Task<Passenger> AddAsync(Passenger entity)
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(int id)
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Passenger>> GetAllAsync()
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }

    public async Task<Passenger?> GetByIdAsync(int id)
    {
        return await _context.Passengers
                   .Include(p => p.Bookings)
                   .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task UpdateAsync(Passenger entity)
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }
}
