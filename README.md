## Features

- **Flight Management**: Create, retrieve, and update flight information
- **Booking System**: Create bookings with seat availability validation
- **Passenger Management**: Handle passenger information and validation
- **CQRS Pattern**: Command Query Responsibility Segregation for better scalability
- **Event Sourcing**: Track changes and events for audit purposes
- **In-Memory Caching**: High-performance caching for flight listings
- **Global Exception Handling**: Comprehensive error handling with proper HTTP status codes
- **Swagger/OpenAPI**: Complete API documentation
- **Unit Tests**: Comprehensive test coverage using xUnit

## Prerequisites

- .NET 8 SDK
- In-Memory DataBase
- In-Memory Caching

## API Endpoints

### Flights

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/flights` | Create a new flight |
| `GET` | `/api/flights` | Get flights with optional filters |
| `PUT` | `/api/flights/{id}/seats` | Update available seats for a flight |

### Bookings

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/bookings` | Create a new booking |
| `GET` | `/api/bookings/flights/{flightId}` | Get all bookings for a specific flight |
