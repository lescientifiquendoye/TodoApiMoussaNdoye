# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Restore dependencies
dotnet restore

# Build
dotnet build

# Run (HTTP on port 5175)
dotnet run --project TodoApiMoussaNdoye --launch-profile http

# Run (HTTPS on port 7255)
dotnet run --project TodoApiMoussaNdoye --launch-profile https
```

No test project exists yet. If added, run tests with `dotnet test`.

## Architecture

**ASP.NET Core 10.0 minimal API** — no MVC controllers, all endpoints defined directly in `Program.cs` using `app.Map*()` methods.

- `Program.cs` — entry point, service registration, and all route definitions
- `appsettings.json` / `appsettings.Development.json` — configuration
- `TodoApiMoussaNdoye.http` — REST client file for manual endpoint testing

**Key packages:** `Microsoft.AspNetCore.OpenApi` (Swagger/OpenAPI docs available at `/openapi` in development).

**Nullable reference types** are enabled — always handle nullability explicitly.
