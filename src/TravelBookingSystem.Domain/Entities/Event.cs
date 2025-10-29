namespace TravelBookingSystem.Domain.Entities;

public class Event : BaseEntity
{
    public string AggregateId { get; private set; } = string.Empty;
    public string AggregateType { get; private set; } = string.Empty;
    public string EventType { get; private set; } = string.Empty;
    public string EventData { get; private set; } = string.Empty;
    public DateTime OccurredAt { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public int Version { get; private set; }

    private Event() { }

    public Event(string aggregateId, string aggregateType, string eventType, string eventData, string userId, int version)
    {
        AggregateId = aggregateId;
        AggregateType = aggregateType;
        EventType = eventType;
        EventData = eventData;
        UserId = userId;
        Version = version;
        OccurredAt = DateTime.UtcNow;
    }
}