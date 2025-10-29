## Architecture Overview

This project follows **Clean Architecture** with clear separation of concerns across four main layers:

- **Domain Layer**: Core business entities, interfaces, and domain events
- **Application Layer**: Business logic, DTOs, CQRS commands/queries, and validation
- **Infrastructure Layer**: Data access, repositories, caching, and external services
- **API Layer**: Controllers, middleware, and application configuration
  
## Features

- **Flight Management**: Create, retrieve, and update flight information
- **Booking System**: Create bookings with seat availability validation
- **Passenger Management**: Handle passenger information and validation
- **CQRS Pattern**: Command Query Responsibility Segregation for better scalability
- **MediatR Pattern**: Encourages clean architecture and CQRS-style organization
- **Event Sourcing**: Track changes and events for audit purposes
- **In-Memory Caching**: High-performance caching for flight listings
- **Global Exception Handling**: Comprehensive error handling with proper HTTP status codes
- **Swagger/OpenAPI**: Complete API documentation
- **Unit Tests**: Comprehensive test coverage using xUnit

## Prerequisites

- .NET 8 SDK
- In-Memory DataBase
- In-Memory Caching
- Docker (optional, for containerized deployment)

## Setup Instructions

### Option1 : Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/MaryamTaghavi/TravelBookingSystem.git
   cd maryamTaghavi-task
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   cd src/TravelBookingSystem.API
   dotnet run
   ```

4. **Access the API**
   - API: `https://localhost:7131` or `http://localhost:5031`
   - Swagger UI: `https://localhost:7131/swagger` or `http://localhost:5031/swagger`

### Option 2: Docker Deployment

1. **Build and run with Docker Compose**
   ```bash
   docker-compose up --build
   ```

2. **Access the application**
   - API: `http://localhost:8080`
   - Swagger UI: `http://localhost:8080`
     
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

## Project Structure

```
TravelBookingSystem/
├── src/
│   ├── TravelBookingSystem.API/           # Web API layer
│   │   ├── Controllers/                    # API controllers
│   │   ├── Middleware/                     # Custom middleware
│   │   └── Program.cs                      # Application entry point
│   ├── TravelBookingSystem.Application/   # Application layer
│   │   ├── DTOs/                          # Data Transfer Objects
│   │   ├── Features/                      # CQRS commands and queries and validators
│   │   └── Mappings/                      # AutoMapper profiles
│   ├── TravelBookingSystem.Domain/        # Domain layer
│   │   ├── Entities/                      # Domain entities
│   │   ├── Interfaces/                    # Repository interfaces
│   │   └── Events/                        # Domain events
│   └── TravelBookingSystem.Infrastructure/ # Infrastructure layer
│       ├── Data/                          # DbContext and configurations
│       ├── Repositories/                  # Repository implementations
│       └── Services/                      # External services
└──
```

## Security Features

- Input validation using FluentValidation
- Global exception handling
- Proper HTTP status codes
- Request/response logging

## Performance Features

- In-Memory caching for frequently accessed data
- Async/await pattern throughout
- Entity Framework optimizations
