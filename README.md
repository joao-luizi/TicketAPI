# Ticketing API

A ticket-management REST API built with **ASP.NET Core (.NET 10)**, following a **Clean Architecture** (layered) approach with a use-case-driven Application layer, EF Core (PostgreSQL) persistence, JWT-based authentication, and Serilog logging.

This project is being developed incrementally as part of an onboarding challenge, evolving step by step through containerization, database integration, validation, authentication, and testing.

---

## Table of Contents

- [Architecture](#architecture)
- [Features](#features)
- [Domain Model](#domain-model)
- [Requirements](#requirements)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database & Migrations](#database--migrations)
- [API Reference](#api-reference)
- [Testing](#testing)
- [License](#license)

---

## Architecture

The solution follows a Clean Architecture / layered design, split into four projects plus a test project:

```
Ticketing.slnx
├── Presentation/Ticketing.Api            # ASP.NET Core Web API (startup project)
│   ├── Controllers/                      # HTTP endpoints (Auth, User, Ticket, Health)
│   ├── Contracts/Request & Response       # DTOs exposed over HTTP
│   ├── Validators/                        # FluentValidation rules for incoming requests
│   └── Program.cs                         # App bootstrap, middleware, migrations on startup
├── Application/Ticketing.Application     # Use cases and application abstractions
│   ├── Abstractions/Persistence           # Repository interfaces (ITicketRepository, IUserRepository, IDbSeeder)
│   ├── Abstractions/Security              # IPasswordHasher, ITokenService
│   └── UseCases/                          # One class per use case (Login, CreateTicket, CreateUser, ...)
├── Domain/Ticketing.Domain               # Framework-free domain models and enums
│   ├── Models/                            # Ticket, User
│   └── Enums/                             # TicketStatus, UserRoles, CreateTicketFailureType
├── Infrastructure/Ticketing.Infrastructure # EF Core implementation details
│   ├── Persistence/Context                # TicketingDbContext, DbSeeder
│   ├── Persistence/Repositories           # EF Core repository implementations
│   ├── Persistence/Configurations         # Entity type configurations (Fluent API)
│   ├── Migrations/                        # EF Core Code-First migrations
│   └── Services/Security                  # BCrypt password hasher, JWT token service
└── UnitTests/Ticketing.Api.Tests         # Controller and validator unit tests
```

Dependencies flow inward: **Presentation → Application → Domain**, with **Infrastructure** plugging into the Application layer through interfaces (dependency inversion). Each layer exposes an `AddXxx` extension method (`AddApi`, `AddApplication`, `AddInfra`) used to wire its services into the DI container from `Program.cs`.

---

## Features

- ✅ RESTful API for ticket management, built on ASP.NET Core / .NET 10
- ✅ Clean Architecture with a use-case-per-class Application layer
- ✅ PostgreSQL persistence via EF Core (Code First + Migrations), applied automatically on startup
- ✅ Automatic database seeding of a default admin user
- ✅ JWT access-token issuance on login (HMAC-SHA256), with role claims (`User` / `Admin`)
- ✅ Password hashing with BCrypt
- ✅ Role-restricted endpoints (e.g. user creation requires the `Admin` role)
- ✅ Request validation with FluentValidation
- ✅ Structured logging with Serilog (console + rolling daily log files)
- ✅ Health-check endpoint
- ✅ Swagger / OpenAPI documentation (Swashbuckle)
- ✅ Fully containerized with Docker/Podman Compose (API + PostgreSQL)
- ✅ Unit tests for controllers and validators

---

## Domain Model

**User**
| Field | Type | Notes |
|---|---|---|
| `Id` | int | |
| `UserName` | string | Unique |
| `Email` | string | Unique |
| `PasswordHash` | string | BCrypt hash |
| `UserRoles` | enum | `User` (0, default) or `Admin` (1) |
| `IsActive` | bool | Defaults to `true` |
| `CreatedAt` | DateTime (UTC) | |

**Ticket**
| Field | Type | Notes |
|---|---|---|
| `Id` | int | |
| `Title` | string | |
| `Description` | string | |
| `UserName` / `UserEmail` | string | Denormalized from the owning user |
| `TicketStatus` | enum | `Created` (0, default), `Closed` (1), `Canceled` (2) |
| `CreatedAt` | DateTime (UTC) | |
| `ClosedAt` | DateTime? | Nullable |

A ticket cannot be created for a user that doesn't exist or is inactive, and duplicate ticket titles for the same user are rejected (`CreateTicketFailureType`: `UserNotFound`, `UserInactive`, `DuplicateTicket`).

---

## Requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)
- Docker or Podman (with `docker compose` / `podman compose`)
- EF Core CLI tools (`dotnet tool install --global dotnet-ef`) — only needed if you want to create/apply migrations manually

---

## Getting Started

### Run everything with Docker/Podman Compose (recommended)

This starts the API and a PostgreSQL container together:

```bash
docker compose up --build
# or
podman compose up --build
```

`--build` is only required the first time, or after changing the API source code. To stop:

```bash
docker compose down
```

To wipe the database completely (removes the Postgres volume):

```bash
docker compose down -v
```

Once running, the API is reachable at `http://localhost:3000`.

### Run locally with `dotnet run`

1. Start only the PostgreSQL container:
   ```bash
   docker compose up db
   ```
2. Run the API from the `Presentation/Ticketing.Api` project:
   ```bash
   dotnet run --project Presentation/Ticketing.Api
   ```
   The API listens on `http://localhost:5012` (see `launchSettings.json`).

Either way, on startup the API automatically applies any pending EF Core migrations and seeds a default admin user (see [Database & Migrations](#database--migrations)).

---

## Configuration

Configuration lives in `Presentation/appsettings.json` (and `appsettings.Development.json`), and can be overridden with environment variables in the standard ASP.NET Core way.

| Section | Key | Description |
|---|---|---|
| `ConnectionStrings` | `TicketingDB` | PostgreSQL connection string |
| `Jwt` | `Key`, `Issuer`, `Audience`, `ExpiryMinutes` | JWT signing configuration used to issue access tokens |
| `Serilog` | `MinimumLevel`, `WriteTo` | Logging sinks (Console, rolling file under `logs/log-.txt`) |

The connection host differs depending on where the API runs:

| Context | DB Host |
|---|---|
| Local (`dotnet run` / migrations from your machine) | `localhost` |
| Containerized (API container → DB container) | `db` |

> The default `Jwt:Key` and Postgres credentials in `appsettings.json` are development-only placeholders — replace them before deploying anywhere real.

---

## Database & Migrations

The project uses EF Core **Code First** with migrations, targeting PostgreSQL (via `Npgsql`).

**Create a new migration:**
```bash
dotnet ef migrations add <MigrationName> \
  --project Infrastructure/Ticketing.Infrastructure/Ticketing.Infrastructure.csproj \
  --startup-project Presentation/Ticketing.Api/Ticketing.Api.csproj \
  --context TicketingDbContext
```

**Apply migrations manually:**
```bash
dotnet ef database update \
  --project Infrastructure/Ticketing.Infrastructure/Ticketing.Infrastructure.csproj \
  --startup-project Presentation/Ticketing.Api/Ticketing.Api.csproj \
  --context TicketingDbContext
```

**Automatic migration on startup:** `Program.cs` calls `db.Database.MigrateAsync()` before the app starts serving requests, so the database schema is always up to date and the database itself is created if it doesn't exist yet.

**Seeding:** on the same startup path, `IDbSeeder` ensures a default administrator account exists (`admin` / `admin@admin.com`, password `!#123Admin`) so there's always at least one `Admin` user able to create further users. This is a development convenience and should not be relied on in production without changing the password.

---

## API Reference

Interactive Swagger UI is available at `http://localhost:3000/swagger` (or the equivalent local port) in every environment, since this is an API-only service with no browsable UI to protect.

| Method | Route | Auth | Description |
|---|---|---|---|
| `GET` | `/health` | — | Returns service status and current UTC time |
| `POST` | `/auth/login` | — | Authenticates a user and returns a JWT access token |
| `POST` | `/users` | `Admin` role | Creates a new user account |
| `POST` | `/tickets` | — | Creates a new ticket on behalf of an existing, active user |

**`POST /auth/login`**
```json
{ "userName": "admin", "password": "!#123Admin" }
```
Returns `200 OK` with `{ success, token, detail }`, or `401 Unauthorized` if the credentials are invalid.

**`POST /users`** *(requires an `Admin` bearer token)*
```json
{ "username": "jdoe", "email": "jdoe@example.com", "password": "P@ssw0rd1", "fullName": "John Doe" }
```
Returns `201 Created` with the new user's id, or `400 Bad Request` if the username/email is already taken or fails validation (username 3–80 chars, valid email, full name 3–160 chars, password ≥ 8 chars with upper/lower case, digit and special character).

**`POST /tickets`**
```json
{ "title": "Printer not working", "description": "Office printer on 3rd floor is jammed", "userEmail": "jdoe@example.com" }
```
Returns `201 Created` with the new ticket's id, `400 Bad Request` for validation errors or an unknown/inactive user, or `409 Conflict` if the same user already has an open ticket with that title.

---

## Testing

Unit tests (controllers and validators) live in `UnitTests/Ticketing.Api.Tests` and can be run with:

```bash
dotnet test
```

---

## License

Distributed under the MIT License. See [`LICENSE`](./LICENSE) for details.