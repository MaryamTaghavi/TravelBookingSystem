using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TravelBookingSystem.Domain.Entities;
using TravelBookingSystem.Domain.Interfaces;

namespace TravelBookingSystem.Infrastructure.Repositories;

public class EventStore : IEventStore
{
    private readonly AppDbContext _context;

    public EventStore(AppDbContext context) => _context = context;

    public async Task SaveEventAsync(string aggregateId, string aggregateType, string eventType, object eventData, string userId, int version)
    {
        var eventJson = JsonSerializer.Serialize(eventData, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var eventRecord = new Event(aggregateId, aggregateType, eventType, eventJson, userId, version);

        _context.Events.Add(eventRecord);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(string aggregateId)
    {
        return await _context.Events
            .Where(e => e.AggregateId == aggregateId)
            .OrderBy(e => e.Version)
            .ToListAsync();
    }
}