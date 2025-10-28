using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using TravelBookingSystem.Api.Middleware;
using TravelBookingSystem.Application.Features.Flights.Commands.Create;
using TravelBookingSystem.Domain.Interfaces;
using TravelBookingSystem.Infrastructure;
using TravelBookingSystem.Infrastructure.Repositories;
using TravelBookingSystem.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MyTestDb"));

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/travel-booking-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

// Inject Services
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CreateFlightCommandHandler).Assembly));

builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    c.SwaggerDoc("v1", new()
    {
        Title = "Travel Booking System API",
        Version = "v1",
        Description = "A comprehensive REST API for managing flight bookings, passengers, and flights."
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
