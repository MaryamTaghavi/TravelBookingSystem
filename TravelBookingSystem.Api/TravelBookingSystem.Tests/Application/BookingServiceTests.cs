using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Infrastructure.Repositories;
using TravelBookingSystem.Infrastructure;

namespace TravelBookingSystem.Tests.Application;

public class BookingServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IBookingRepository _bookingRepository;

    public BookingServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MyTestDb")
            .Options;

        _context = new AppDbContext(options);
        _bookingRepository = new BookingRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
