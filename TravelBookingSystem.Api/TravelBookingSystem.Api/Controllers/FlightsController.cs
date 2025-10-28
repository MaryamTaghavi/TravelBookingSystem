using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelBookingSystem.Application.Features.Flights.Commands.Create;
using TravelBookingSystem.Application.DTOs;

namespace TravelBookingSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlightsController : ControllerBase
{
    private readonly ISender _mediatr;

    /// <summary>
    /// FlightsController
    /// </summary>
    /// <param name="mediatr"></param>
    public FlightsController(ISender mediatr) => _mediatr = mediatr;

    /// <summary>
    /// Create Flight
    /// </summary>
    /// <remarks>
    /// ## Format Date
    /// "14:20 1404/01/01"
    /// </remarks>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFlightCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _mediatr.Send(command, cancellationToken);
        return Ok(result);
    }
}
