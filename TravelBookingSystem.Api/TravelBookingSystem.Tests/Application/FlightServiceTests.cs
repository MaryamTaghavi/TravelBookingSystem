using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TravelBookingSystem.Application.Features.Flights.Commands.Create;
using TravelBookingSystem.Domain.Entities;
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

    [Fact]
    public async Task CreateFlight_WithDuplicateFlightNumber_ShouldThrowException()
    {
        var loggerMock = new Mock<ILogger<CreateFlightCommandHandler>>();
        var eventStoreMock = new Mock<IEventStore>();
        var cacheServiceMock = new Mock<ICacheService>();

        // Arrange
        var existingFlight = new Flight("123456", "Yazd", "Mashhad",
            DateTime.UtcNow.AddDays(3), DateTime.UtcNow.AddDays(3).AddHours(2),
            120, 4_000_000 , BitConverter.GetBytes(DateTime.Now.Ticks));

        await _flightRepository.AddAsync(existingFlight);

        var command = new CreateFlightCommand
        {
            FlightNumber = "123456", // Same flight number
            Origin = "Yazd",
            Destination = "Mashhad",
            DepartureTime = DateTime.UtcNow.AddDays(4),
            ArrivalTime = DateTime.UtcNow.AddDays(4).AddHours(2),
            AvailableSeats = 100,
            Price = 4_000_000,
        };

        var handler = new CreateFlightCommandHandler(_flightRepository, loggerMock.Object,
                            eventStoreMock.Object, cacheServiceMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("Flight number already exists");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
