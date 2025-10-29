using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelBookingSystem.Domain.Entities;

namespace TravelBookingSystem.Infrastructure;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (!context.Flights.Any())
        {
            context.Flights.AddRange(
                new Flight("123456", "Tehran", "Istanbul",
                    DateTime.UtcNow.AddDays(1),
                    DateTime.UtcNow.AddDays(1).AddHours(3),
                    100, 2_000_000, BitConverter.GetBytes(DateTime.Now.Ticks)),

                new Flight("123457", "Shiraz", "Dubai",
                    DateTime.UtcNow.AddDays(2),
                    DateTime.UtcNow.AddDays(2).AddHours(2),
                    150, 3_500_000, BitConverter.GetBytes(DateTime.Now.Ticks))
            );

            context.SaveChanges();
        }

        if (!context.Passengers.Any())
        {
            context.Passengers.AddRange(
                new Passenger("MaryamTaghavi", "m.taghavi@example.com", "987654321", "09103160108"),
                new Passenger("ParsaTaghavi", "parsa@example.com", "123456789", "09133537911")
            );

            context.SaveChanges();
        }
    }
}
