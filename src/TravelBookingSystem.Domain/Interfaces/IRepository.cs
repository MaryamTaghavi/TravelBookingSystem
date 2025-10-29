using Microsoft.EntityFrameworkCore.Storage;

namespace TravelBookingSystem.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id , CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity , CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id , CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id , CancellationToken cancellationToken = default);
}

public interface IFlightRepository : IRepository<Entities.Flight>
{
    Task<IEnumerable<Entities.Flight>> GetFlightsByFiltersAsync(string? origin, string? destination, DateTime? date , CancellationToken cancellationToken = default);
    Task<Entities.Flight?> GetByFlightNumberAsync(string flightNumber, CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}

public interface IBookingRepository : IRepository<Entities.Booking>
{
    Task<IEnumerable<Entities.Booking>> GetBookingsByFlightIdAsync(int flightId , CancellationToken cancellationToken = default);
    Task<bool> IsSeatAvailableAsync(int flightId, string seatNumber, CancellationToken cancellationToken = default);
}

public interface IPassengerRepository : IRepository<Entities.Passenger>
{
    // TODO : Add methods if need
}