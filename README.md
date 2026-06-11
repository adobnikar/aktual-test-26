# Address Book

Fullstack "Address book" web application — view, search, create, update and delete contacts.

## Quick start

### Run everything with Docker

```bash
docker compose -f docker-compose.full.yml up --build
```

Open <http://localhost:8080>. The database schema and ~30 seed contacts are created automatically on startup. Override the port with `APP_PORT`, database credentials with `POSTGRES_DB` / `POSTGRES_USER` / `POSTGRES_PASSWORD`.

### Local development

Prerequisites: .NET SDK 8, Node.js 22+, Docker.

```bash
# 1. Start the development database (PostgreSQL on host port 5434)
docker compose up -d

# 2. Run the API (http://localhost:5289, applies migrations + seed)
cd backend/AddressBook.Api
dotnet run

# 3. Run the frontend (http://localhost:4200, proxies /api to the backend)
cd frontend
npm install
npm start
```

Database defaults (overridable via `POSTGRES_*` environment variables): host `localhost`, port `5434`, database/user/password `addressbook`.

## About

- **Backend:** .NET 8 Web API, EF Core, PostgreSQL, FluentValidation
- **Frontend:** Angular 19, Bootstrap 5, ng-bootstrap

See [TASK.md](TASK.md) for the full assignment requirements.

## Architecture

```
backend/
├── AddressBook.Api/             # REST controllers, error-handling middleware, DI wiring, Swagger
├── AddressBook.Application/     # services, DTOs, validators, repository interfaces (no EF dependency)
├── AddressBook.Infrastructure/  # EF Core DbContext, repository implementation, migrations, seed
└── AddressBook.Api.Tests/       # unit tests + API integration tests (Testcontainers)
frontend/
└── src/app/
    ├── core/                    # models, ContactService (API client), error interceptor + dialog service
    ├── features/contacts/       # contact-list, contact-detail, contact-form components
    └── shared/                  # confirm modal, error modal
```

**Backend.** Classic layered design: controllers depend on `IContactService`, the service on `IContactRepository`, all wired through the built-in DI container. Requests are validated with FluentValidation (all fields required, phone format); phone-number uniqueness is checked in the service and additionally enforced by a unique database index, with the constraint violation translated to a conflict response. A middleware converts every error into an RFC 7807 problem-details response with a standard status code (400 validation / 404 not found / 409 conflict / 500 unexpected).

**Search & pagination** happen in the database: each contact field is filtered independently with a case-insensitive contains match (`ILIKE`, wildcards escaped), combined with AND, then paged with a total count — the frontend never filters client-side.

**Frontend.** Standalone components with lazy routes. The list view debounces per-column search inputs and sends them as query parameters together with the page number. Create and edit share one reactive-form component mirroring the backend rules; a server 409 is mapped onto the phone-number field. Deleting always asks for confirmation in a modal, and an HTTP interceptor shows unexpected server errors in a global error modal. The app calls a relative `/api` everywhere — `ng serve` proxies it to the backend in development, nginx does in the Docker setup — so no API URL is baked into the build.

## REST API

| Method & path | Description | Status codes |
|---|---|---|
| `GET /api/contacts` | List; filters `firstName`, `lastName`, `address`, `phoneNumber` + `page`, `pageSize` | 200 |
| `GET /api/contacts/{id}` | Single contact | 200, 404 |
| `POST /api/contacts` | Create | 201, 400, 409 |
| `PUT /api/contacts/{id}` | Update | 200, 400, 404, 409 |
| `DELETE /api/contacts/{id}` | Delete | 204, 404 |

Swagger UI is available at `/swagger` when the API runs in the Development environment.

## Tests

```bash
cd backend
dotnet test
```

Runs 39 tests: validator and service unit tests, plus integration tests that boot the full API against a throwaway PostgreSQL container (requires Docker).

## Troubleshooting

On old Docker daemons (≤ 20.10) the default seccomp profile blocks syscalls .NET 8 needs, breaking both image builds and containers. The compose file already runs the API with `seccomp:unconfined`; for building, either upgrade Docker or build with a containerized BuildKit builder: `docker buildx create --driver docker-container --use`.
