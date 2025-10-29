using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TravelBookingSystem.Application.Features.Flights.Commands.Create;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Infrastructure;
using TravelBookingSystem.Infrastructure.Repositories;
using Xunit;

namespace TravelBookingSystem.Tests.Application;

public class FlightServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IFlightRepository _flightRepository;

    public FlightServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MyTestDb")
            .Options;

        _context = new AppDbContext(options);
        _flightRepository = new FlightRepository(_context);
    }

    [Fact]
    public async Task CreateFlight_WithValidData_ShouldSucceed()
    {
        var loggerMock = new Mock<ILogger<CreateFlightCommandHandler>>();
        var eventStoreMock = new Mock<IEventStore>();
        var cacheServiceMock = new Mock<ICacheService>();

        // Arrange
        var command = new CreateFlightCommand
        {
            FlightNumber = "123456",
            Origin = "Yazd",
            Destination = "Mashhad",
            DepartureTime = DateTime.UtcNow.AddDays(3),
            ArrivalTime = DateTime.UtcNow.AddDays(3).AddHours(2),
            AvailableSeats = 120,
            Price = 4_000_000
        };

        var handler = new CreateFlightCommandHandler(_flightRepository, loggerMock.Object,
                            eventStoreMock.Object, cacheServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.FlightNumber.Should().Be("123456");
        result.Origin.Should().Be("Yazd");
        result.Destination.Should().Be("Mashhad");
        result.AvailableSeats.Should().Be(120);
        result.Price.Should().Be(4_000_000);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
