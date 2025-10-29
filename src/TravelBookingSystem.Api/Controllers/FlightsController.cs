using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelBookingSystem.Application.Features.Flights.Commands.Create;
using TravelBookingSystem.Application.DTOs;
using TravelBookingSystem.Application.Features.Flights.Commands.Update;
using TravelBookingSystem.Application.Features.Flights.Queries.GetFlights;

namespace TravelBookingSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightsController : ControllerBase
{
    private readonly ISender _mediator;

    /// <summary>
    /// FlightsController
    /// </summary>
    /// <param name="mediator"></param>
    public FlightsController(ISender mediator) => _mediator = mediator;

    /// <summary>
    /// Create flight
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FlightDto>> Create([FromBody] CreateFlightCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetFlights), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get flights with optional filters
    /// </summary>
    /// <param name="origin">Filter by origin city</param>
    /// <param name="destination">Filter by destination city</param>
    /// <param name="date">Filter by departure date</param>
    /// <param name="cancellationToken">Filter by departure date</param>
    /// <returns>List of flights</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FlightDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FlightDto>>> GetFlights(
        [FromQuery] string? origin = null,
        [FromQuery] string? destination = null,
        [FromQuery] DateTime? date = null , 
        CancellationToken cancellationToken = default)
    {
        var query = new GetFlightsQuery
        {
            Origin = origin,
            Destination = destination,
            Date = date
        };

        var result = await _mediator.Send(query , cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update available seats for a flight
    /// </summary>
    /// <param name="id">Flight ID</param>
    /// <param name="command">Seat update details</param>
    /// <param name="cancellationToken">Filter by departure date</param>
    /// <returns>Updated flight information</returns>
    [HttpPut("{id}/seats")]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightDto>> UpdateFlightSeats(int id, [FromBody] UpdateFlightSeatsCommand command, CancellationToken cancellationToken = default)
    {
        command.FlightId = id;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
