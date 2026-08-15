# HotelBooking

Lightweight hotel room booking API and sample application built with .NET 10.

## Overview

HotelBooking is a sample service demonstrating a layered architecture for managing hotels, rooms and bookings. The solution includes:
- HotelBooking.Api — ASP.NET Core Web API (controllers and DTOs)
- HotelBooking.Application — application services and interfaces
- HotelBooking.Domain — domain entities and enums
- HotelBooking.Infrastructure — repository implementations
- Tests — unit/integration tests for API, services and repositories

This repository is intended for learning, experimentation, and as a starting point for production services.

## Key Features

- Create and query bookings by reference
- Room availability checks
- Validation for booking dates, guest counts and room capacity
- Layered design (Controllers → Services → Repositories)
- Unit tests for controllers, services and repositories

## Prerequisites

- .NET 10 SDK
- Visual Studio 2026 (recommended) or VS Code

## Getting started (CLI)

1. Clone the repository
   - git clone https://github.com/DJSavage/HotelBooking.git
   - cd HotelBooking

2. Restore and build
   - dotnet restore
   - dotnet build

3. Configure the app
   - Update connection strings / settings in the API project's `appsettings.json` or user secrets if needed.

4. Run the API
   - From solution root:
	 - dotnet run --project HotelBooking.Api
   - Or open the solution in Visual Studio 2026 and run the API project.

5. Explore the API (recommended)
   - Swagger UI (recommended): when the API is running, open the Swagger UI at `http://localhost:{port}/swagger` to browse endpoints, view schemas, and execute requests interactively.
   - Alternatively use curl or Postman if you prefer, but Swagger gives an integrated interactive experience.

## Running tests

- Run all tests:
  - dotnet test

- In Visual Studio: Test Explorer → Run All

## Debugging tips

- The solution is set up for debugging in Visual Studio 2026. Set breakpoints in controllers/services and run the API or test project under the debugger.

## Contributing

- Fork the repository and submit pull requests.
- Follow existing code style and add unit tests for new features.

## License

Add a license file (e.g., MIT) to the repository root to clarify reuse terms.

## Contact

Open an issue on the repository for questions or clarifications.
