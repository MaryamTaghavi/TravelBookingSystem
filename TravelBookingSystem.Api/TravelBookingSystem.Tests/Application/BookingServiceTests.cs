using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TravelBookingSystem.Application.Features.Bookings.Create;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Infrastructure;
using TravelBookingSystem.Infrastructure.Repositories;
using Xunit;

namespace TravelBookingSystem.Tests.Application;

public class BookingServiceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IPassengerRepository _passengerRepository;

    public BookingServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "MyTestDb")
            .Options;

        _context = new AppDbContext(options);
        _bookingRepository = new BookingRepository(_context);
        _flightRepository = new FlightRepository(_context);
        _passengerRepository = new PassengerRepository(_context);
    }

    [Fact]
    public async Task CreateBooking_WithValidData_ShouldSucceed()
    {
        var loggerMock = new Mock<ILogger<CreateBookingCommandHandler>>();
        var eventStoreMock = new Mock<IEventStore>();

        SeedData();

        // Arrange
        var command = new CreateBookingCommand
        {
            FlightId = 1,
            PassengerId = 1,
            SeatNumber = "A1"
        };

        var handler = new CreateBookingCommandHandler(
            _bookingRepository, _flightRepository,
            _passengerRepository, eventStoreMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SeatNumber.Should().Be("A1");
        result.FlightNumber.Should().Be("123456");
        result.PassengerName.Should().Be("MaryamTaghavi");
    }

    [Fact]
    public async Task CreateBooking_WithOccupiedSeat_ShouldThrowException()
    {
        var loggerMock = new Mock<ILogger<CreateBookingCommandHandler>>();
        var eventStoreMock = new Mock<IEventStore>();

        SeedData();

        // Arrange
        // First booking
        var firstCommand = new CreateBookingCommand
        {
            FlightId = 1,
            PassengerId = 1,
            SeatNumber = "A1"
        };

        var handler = new CreateBookingCommandHandler(
            _bookingRepository, _flightRepository,
            _passengerRepository, eventStoreMock.Object);

        await handler.Handle(firstCommand, CancellationToken.None);

        // Second booking with same seat
        var secondCommand = new CreateBookingCommand
        {
            FlightId = 1,
            PassengerId = 2,
            SeatNumber = "A1"
        };

        SeedData();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(secondCommand, CancellationToken.None));

        exception.Message.Should().Contain("Seat is already occupied");
    }

    [Fact]
    public async Task CreateBooking_WithNonExistentFlight_ShouldThrowException()
    {
        var loggerMock = new Mock<ILogger<CreateBookingCommandHandler>>();
        var eventStoreMock = new Mock<IEventStore>();

        await SeedData();

        // Arrange
        var command = new CreateBookingCommand
        {
            FlightId = 999, // Non-existent flight
            PassengerId = 1,
            SeatNumber = "A1"
        };

        var handler = new CreateBookingCommandHandler(
                _bookingRepository, _flightRepository,
                _passengerRepository, eventStoreMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("Flight not found");
    }

    [Fact]
    public async Task CreateBooking_WithNonExistentPassenger_ShouldThrowException()
    {
        var loggerMock = new Mock<ILogger<CreateBookingCommandHandler>>();
        var eventStoreMock = new Mock<IEventStore>();

        await SeedData();

        // Arrange
        var command = new CreateBookingCommand
        {
            FlightId = 1,
            PassengerId = 999, // Non-existent passenger
            SeatNumber = "A1"
        };

        var handler = new CreateBookingCommandHandler(
                _bookingRepository, _flightRepository,
                _passengerRepository, eventStoreMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Contain("Passenger not found");
    }

    private async Task SeedData()
    {
        // Seed Flight
        var flight = new Flight("123456", "Yazd", "Mashhad",
            DateTime.UtcNow.AddDays(3), DateTime.UtcNow.AddDays(3).AddHours(2),
            120, 4_000_000, BitConverter.GetBytes(DateTime.Now.Ticks));

        await _flightRepository.AddAsync(flight);

        // Seed Passenger
        var passenger = new Passenger("MaryamTaghavi", "m.taghavi.ce@gmail.com", "987654", "09103160108");

        await _passengerRepository.AddAsync(passenger);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
