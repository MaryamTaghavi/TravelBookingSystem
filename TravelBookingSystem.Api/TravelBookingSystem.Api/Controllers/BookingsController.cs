using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Application.Features.Bookings.Create;

namespace TravelBookingSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly ISender _mediator;

    /// <summary>
    /// BookingsController
    /// </summary>
    /// <param name="mediator"></param>
    public BookingsController(ISender mediator) => _mediator = mediator;

    /// <summary>
    /// Create a new booking
    /// </summary>
    /// <param name="command">Booking creation details</param>
    /// <returns>Created booking information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingResponseDto>> CreateBooking([FromBody] CreateBookingCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBookingsByFlightId), new { flightId = command.FlightId }, result);
    }

    /// <summary>
    /// Get all bookings for a specific flight
    /// </summary>
    /// <param name="flightId">Flight ID</param>
    /// <returns>List of bookings for the flight</returns>
    [HttpGet("flights/{flightId}")]
    [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByFlightId(int flightId)
    {
        // TODO : must complete 
        return null;
        //var result = await _mediator.Send(query);
        //return Ok(result);
    }
}
