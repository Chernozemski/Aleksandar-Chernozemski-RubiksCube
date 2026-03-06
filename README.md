# Rubik's Cube Simulator

## Backend (ASP.NET)

- **CLI:** From the solution root, run `dotnet run --project src/RubiksCube.Api`. The API listens on `http://localhost:5205`.
- **Visual Studio:** Open `RubiksCube.slnx`, set RubiksCube.Api as the startup project, and run (F5).

## Frontend (React)

- Run `npm install` once in `src/RubiksCube.Web`.
- Then run `npm run dev` for development or `npm run build` for a production build.
- The dev server runs on a different port than the API (e.g. `http://localhost:65142`). Configure the API base URL in `src/RubiksCube.Web/.env.development` (`VITE_API_URL`) and ensure CORS on the API allows that origin (see `appsettings.Development.json`).

## Tests

- **Backend:** `dotnet test tests/RubiksCube.Api.Tests/RubiksCube.Api.Tests.csproj`
- **Frontend:** In `src/RubiksCube.Web`, run `npm run test`.
