#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TravelBookingSystem.Api/TravelBookingSystem.Api.csproj", "TravelBookingSystem.Api/"]
COPY ["TravelBookingSystem.Application/TravelBookingSystem.Application.csproj", "TravelBookingSystem.Application/"]
COPY ["TravelBookingSystem.Domain/TravelBookingSystem.Domain.csproj", "TravelBookingSystem.Domain/"]
COPY ["TravelBookingSystem.Infrastructure/TravelBookingSystem.Infrastructure.csproj", "TravelBookingSystem.Infrastructure/"]
RUN dotnet restore "./TravelBookingSystem.Api/TravelBookingSystem.Api.csproj"
COPY . .
WORKDIR "/src/TravelBookingSystem.Api"
RUN dotnet build "./TravelBookingSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TravelBookingSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TravelBookingSystem.Api.dll"]