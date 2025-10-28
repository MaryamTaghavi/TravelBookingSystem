namespace TravelBookingSystem.Domain.Events;

public abstract class DomainEvent
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    public Guid EventId { get; protected set; } = Guid.NewGuid();
}

public class FlightCreatedEvent : DomainEvent
{
    public int FlightId { get; }
    public string FlightNumber { get; }
    public string Origin { get; }
    public string Destination { get; }

    public FlightCreatedEvent(int flightId, string flightNumber, string origin, string destination)
    {
        FlightId = flightId;
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
    }
}