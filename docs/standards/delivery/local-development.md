# Local Development

## Prerequisites

- .NET 10 SDK

## Build

```bash
cd src
dotnet build
```

## Test

```bash
cd src
dotnet test
```

Runs all test layers: domain TDD, API integration, BDD scenarios, and service tests.

## Run

Each app starts independently with `dotnet run`:

| Terminal | Command | Notes |
|----------|---------|-------|
| 1 | `dotnet run --project src/CompileAndSip.OrderApi` | Start first — listens on `http://localhost:5100` |
| 2 | `dotnet run --project src/CompileAndSip.KioskTui` | Customer ordering terminal |
| 3 | `dotnet run --project src/CompileAndSip.KitchenDisplay` | Staff kitchen display |

Or use the convenience script:

```bash
./src/run-all.sh
```

This starts all three apps and stops them on `Ctrl+C`.

## Configuration

- **Order API** listens on `http://localhost:5100` (configured in `CompileAndSip.OrderApi/Properties/launchSettings.json`).
- **TUI apps** connect to Order API via `OrderApi:BaseUrl` config (default: `http://localhost:5100`).
