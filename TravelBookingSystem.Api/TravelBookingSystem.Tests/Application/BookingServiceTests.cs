using Microsoft.EntityFrameworkCore;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Infrastructure.Repositories;
using TravelBookingSystem.Infrastructure;
using TravelBookingSystem.Application.Features.Bookings.Create;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using TravelBookingSystem.Domain.Entities;

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

        // Seed Flight
        var flight = new Flight("123456", "Yazd", "Mashhad",
            DateTime.UtcNow.AddDays(3), DateTime.UtcNow.AddDays(3).AddHours(2),
            120, 4_000_000, BitConverter.GetBytes(DateTime.Now.Ticks));

        await _flightRepository.AddAsync(flight);

        // Seed Passenger
        var passenger = new Passenger("MaryamTaghavi", "m.taghavi.ce@gmail.com", "987654", "09103160108");

        await _passengerRepository.AddAsync(passenger);

        // Arrange
        var command = new CreateBookingCommand
        {
            FlightId = 1,
            PassengerId = 1,
            SeatNumber = "A1"
        };

        var handler = new CreateBookingCommandHandler(
            _bookingRepository, _flightRepository,
            _passengerRepository , eventStoreMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SeatNumber.Should().Be("A1");
        result.FlightNumber.Should().Be("123456");
        result.PassengerName.Should().Be("Maryam Taghavi");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
