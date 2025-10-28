using Microsoft.EntityFrameworkCore.Storage;

namespace TravelBookingSystem.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IDbContextTransaction> BeginTransactionAsync();
}

public interface IFlightRepository : IRepository<Entities.Flight>
{
    Task<IEnumerable<Entities.Flight>> GetFlightsByFiltersAsync(string? origin, string? destination, DateTime? date);
    Task<Entities.Flight?> GetByFlightNumberAsync(string flightNumber);
}

public interface IBookingRepository : IRepository<Entities.Booking>
{
    Task<bool> IsSeatAvailableAsync(int flightId, string seatNumber);
}

public interface IPassengerRepository : IRepository<Entities.Passenger>
{
    // TODO : Add methods if need
}