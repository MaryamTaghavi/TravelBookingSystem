using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Infrastructure.Repositories;

public class PassengerRepository : IPassengerRepository
{
    private readonly AppDbContext _context;

    public PassengerRepository(AppDbContext context) => _context = context;

    public async Task<Passenger> AddAsync(Passenger entity , CancellationToken cancellationToken)
    {
        await _context.Passengers.AddAsync(entity , cancellationToken);
        return entity;
    }

    public Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Passenger>> GetAllAsync(CancellationToken cancellationToken)
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }

    public async Task<Passenger?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Passengers
                   .Include(p => p.Bookings)
                   .FirstOrDefaultAsync(p => p.Id == id , cancellationToken);
    }

    public void Update(Passenger entity)
    {
        //TODO : must be implement
        throw new NotImplementedException();
    }
}
