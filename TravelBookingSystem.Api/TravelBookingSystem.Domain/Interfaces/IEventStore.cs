using TravelBookingSystem.Domain.Entities;

namespace TravelBookingSystem.Domain.Interfaces;

public interface IEventStore
{
    Task SaveEventAsync(string aggregateId, string aggregateType, string eventType, object eventData, string userId, int version);
    Task<IEnumerable<Event>> GetEventsAsync(string aggregateId);
}