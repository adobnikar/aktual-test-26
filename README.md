# Address Book

Fullstack "Address book" web application — view, search, create, update and delete contacts.

- **Backend:** .NET 8 Web API, EF Core, PostgreSQL
- **Frontend:** Angular, Bootstrap

See [TASK.md](TASK.md) for the full assignment requirements.

## Project structure

```
backend/    # .NET solution (REST API)
frontend/   # Angular application
```

## Local development

Start the development database (PostgreSQL, listening on host port `5434` by default):

```bash
docker compose up -d
```

Default credentials (overridable via `POSTGRES_DB`, `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_PORT` environment variables):

| Setting  | Value         |
|----------|---------------|
| Host     | `localhost`   |
| Port     | `5434`        |
| Database | `addressbook` |
| User     | `addressbook` |
| Password | `addressbook` |

Backend and frontend run instructions will be added as the project takes shape.
