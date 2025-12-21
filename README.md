# Cinema Control (Backend + Frontend)

Clean Architecture cinema management system with a .NET 9 Web API and an Angular standalone frontend. The backend models multi-site cinemas, screenings, seat maps, reservations, and occupancy; the frontend provides public views plus a protected admin area.

## Tech Stack

Backend
- .NET 9 Web API
- PostgreSQL + EF Core
- Clean Architecture (Domain, Application, Infrastructure, API)
- FluentValidation

Frontend
- Angular standalone (v17)
- RxJS + HttpClient
- Component-first UI with shared design system

## First-Time Setup

Prerequisites
- .NET SDK 9
- PostgreSQL
- Node.js (LTS recommended)
- npm

1) Configure backend settings
- Update the connection string in `backend/Cinema.Api/appsettings.Development.json`.
- Set `Security:AdminApiKey` to a strong value.
- Optional: leave `Security:ApiKey` empty to keep public endpoints open.

2) Restore backend tools and apply migrations
```bash
cd backend
dotnet tool restore
dotnet tool run dotnet-ef database update -p Cinema.Infrastructure -s Cinema.Api
```

3) Run the backend
```bash
dotnet run --project backend/Cinema.Api
```

4) Install and run the frontend
```bash
cd frontend
npm install
ng serve
```

Frontend runs at `http://localhost:4200`. Backend runs at `http://localhost:5070` by default.

## Project Structure

Backend
- `backend/Cinema.Domain` — entities, value objects, enums
- `backend/Cinema.Application` — DTOs, validators, business services
- `backend/Cinema.Infrastructure` — EF Core, persistence, background jobs
- `backend/Cinema.Api` — controllers, middleware, configuration
- `backend/Cinema.Tests` — tests

Frontend
- `frontend/src/app/core` — API client, auth, notifications
- `frontend/src/app/features` — public pages and admin pages
- `frontend/src/app/layouts` — public/admin layouts
- `frontend/src/app/shared` — reusable UI components
- `frontend/src/environments` — environment config
- `frontend/src/styles.scss` — design system and global styles

Admin feature structure
- `frontend/src/app/features/admin/pages/*` — admin pages grouped by domain

## How It Works (Flow)

1) Create base data (Admin)
- Use the Admin UI to create cinema sites, auditoriums, seats, movies, and screenings.
- Or call the seed endpoint in development.

2) Public browsing
- Users can browse cinema sites, auditoriums, seats, movies, and screenings.

3) Seat map and occupancy
- Seat map shows available/held/reserved seats for a screening.
- Occupancy aggregates capacity, holds, and reservations.

4) Reservations
- Create a reservation with selected seats (puts seats on hold).
- Confirm or cancel the reservation.
- Holds expire automatically after a configured time window.

## API Security Model

Public endpoints
- All `/api/*` routes are public by default.

Admin endpoints
- `/api/admin/*` requires `Security:AdminApiKey`.
- Frontend Admin login validates the key before allowing access.

Header name:
- `X-Api-Key: <key>`

## Seed Data

Admin seed endpoint (development only):
```http
POST /api/admin/seed
```

The Admin UI includes a "Seed Database" button that calls this endpoint.

## Notes
- Seat holds expire after the configured window (background job).
- Pricing rules live in `backend/Cinema.Api/appsettings.json` under `Pricing`.
- CORS is configured for `http://localhost:4200`.

## Troubleshooting

401 Unauthorized on admin endpoints
- Ensure `Security:AdminApiKey` is set in `backend/Cinema.Api/appsettings.Development.json`.
- Use the Admin login page to validate and store the key.

401 Unauthorized on public endpoints
- Public endpoints are open by default. If `Security:ApiKey` is set, you must provide `X-Api-Key`.

CORS error in the browser
- Confirm the backend is running and CORS allows `http://localhost:4200`.
- Restart the backend after changing CORS settings.

Database migration issues
- Verify PostgreSQL is running and the connection string is correct.
- Re-run: `dotnet tool run dotnet-ef database update -p Cinema.Infrastructure -s Cinema.Api`

Angular build or serve warnings
- Use an LTS Node.js version.
- Run `npm install` again if packages are missing.
