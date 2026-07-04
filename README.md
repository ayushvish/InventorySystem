# InventorySystemPharma

A .NET 10-based inventory system for pharmacy management.

Contents
- src/Inventory.API: Web API project
- docs/API_Flow.md: API flow and design notes

Prerequisites
- .NET 10 SDK
- Visual Studio 2026 (recommended)

Build
- dotnet build

Run
- dotnet run --project src/Inventory.API

Tests
- dotnet test

Notes
- See docs/API_Flow.md for API flow details.

License
- Add a LICENSE file if needed.

Project Flow
------------

Overview
- The project is a layered .NET 10 Web API for pharmacy inventory management. It separates responsibilities into API (controllers), Services (business logic), Repositories (data access), and Infrastructure (persistence, logging, configuration).

Components
- src/Inventory.API: HTTP entrypoint exposing REST endpoints.
- src/Inventory.Application: Business logic and use-cases (services, DTOs).
- src/Inventory.Infrastructure: Data access implementations, EF Core DbContext, migrations, external integrations.
- src/Inventory.Domain: Core entities, value objects, domain rules.

Typical request flow
1. Client sends HTTP request to an API endpoint (e.g., POST /api/medicines).
2. Controller validates input and maps to an application DTO.
3. Controller calls a Service method (application layer) to perform the operation.
4. Service enforces business rules, coordinates transactions, and calls Repository interfaces.
5. Repository implementation (in Infrastructure) uses EF Core to persist or query the database.
6. Service returns result to Controller which maps it to an HTTP response.
7. Controller returns HTTP status and payload to the client.

Data flow example: Add medicine
- Client POST /api/medicines -> Controller -> Validate -> Service.CreateMedicine(dto) -> Repository.Add(entity) -> DbContext.SaveChanges() -> return created resource id.

Cross-cutting concerns
- Logging: centralized logging configured in Infrastructure and wired in Program.cs.
- Error handling: global exception middleware maps exceptions to standardized HTTP responses.
- Configuration: environment-specific settings using appsettings.{Environment}.json and secret management for production.

Authentication & Authorization
- API supports token-based authentication (e.g., JWT). Controllers/actions are decorated with authorization attributes to restrict access by role or policy.

Testing
- Unit tests cover Services and Domain logic.
- Integration tests exercise controllers with an in-memory or test database.

Local development
- Use dotnet watch run --project src/Inventory.API for iterative development.
- Apply EF Core migrations with dotnet ef database update from the Infrastructure project.

Deployment
- Build artifacts and publish the API as a container or deploy to a hosting service (Azure App Service, AKS, etc.).
- Use CI to run tests and produce artifacts.
