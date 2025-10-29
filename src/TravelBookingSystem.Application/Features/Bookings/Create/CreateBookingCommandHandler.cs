using MediatR;
using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Application.Mappings;
using TravelBookingSystem.Domain.Events;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Application.Features.Bookings.Create;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponseDto>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IPassengerRepository _passengerRepository;
    private readonly IEventStore _eventStore;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository,
        IFlightRepository flightRepository,
        IPassengerRepository passengerRepository,
        IEventStore eventStore)
    {
        _bookingRepository = bookingRepository;
        _flightRepository = flightRepository;
        _passengerRepository = passengerRepository;
        _eventStore = eventStore;
    }

    public async Task<BookingResponseDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // Validate flight exists
        var flight = await _flightRepository.GetByIdAsync(request.FlightId , cancellationToken);
        if (flight == null)
        {
            throw new KeyNotFoundException("Flight not found");
        }

        // Validate passenger exists
        var passenger = await _passengerRepository.GetByIdAsync(request.PassengerId, cancellationToken);
        if (passenger == null)
        {
            throw new KeyNotFoundException("Passenger not found");
        }

        // Check if seats are available
        if (!flight.HasAvailableSeats())
        {
            throw new InvalidOperationException("No seats available for this flight");
        }

        // Check if seat is already taken
        var isSeatAvailable = await _bookingRepository.IsSeatAvailableAsync(request.FlightId, request.SeatNumber , cancellationToken);
        if (!isSeatAvailable)
        {
            throw new InvalidOperationException("Seat is already occupied");
        }

        // Error : Because use In-Memory Database can not use begin transaction
        // using var transaction = await _flightRepository.BeginTransactionAsync(cancellationToken);

        try
        {
            // Create booking
            var booking = new Domain.Entities.Booking(request.FlightId, request.PassengerId, request.SeatNumber);
            var createdBooking = await _bookingRepository.AddAsync(booking, cancellationToken);

            // Update available seats
            flight.ReduceAvailableSeats();
            await _flightRepository.UpdateAsync(flight, cancellationToken);

            // Store event
            var bookingCreatedEvent = new BookingCreatedEvent(
                createdBooking.Id,
                createdBooking.FlightId,
                createdBooking.PassengerId,
                createdBooking.SeatNumber
            );

            await _eventStore.SaveEventAsync(
                createdBooking.Id.ToString(),
                "Booking",
                "BookingCreated",
                bookingCreatedEvent,
                "system", // In production, this would come from authentication context
                1,
                cancellationToken
            );

            // await transaction.CommitAsync();
            return createdBooking.ToResponseDto(flight, passenger);
        }

        catch
        {
            // await transaction.RollbackAsync();
            throw new ArgumentException("create booking failed.");
        }
    }
}