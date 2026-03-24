# Compile & Sip — Source

All source code and the solution file for the Compile & Sip Kiosk System.

## Prerequisites

- .NET 10 SDK

The easiest way to get started is to use the included **devcontainer**, which has all dependencies pre-installed. Open the repo in VS Code and select "Reopen in Container" when prompted.

## Project layout

| Project | Type | Description |
|---------|------|-------------|
| `CompileAndSip.OrderApi` | ASP.NET Core Minimal API | Central backend — menu, orders, payment |
| `CompileAndSip.KioskTui` | Console (Spectre.Console) | Customer-facing ordering terminal |
| `CompileAndSip.KitchenDisplay` | Console (Spectre.Console) | Staff-facing kitchen order display |
| `tests/CompileAndSip.OrderApi.Tests` | xUnit | Domain TDD + API integration tests |
| `tests/CompileAndSip.Bdd.Tests` | xUnit + Reqnroll | BDD features for business scenarios (SCN-001–005) |
| `tests/CompileAndSip.KioskTui.Tests` | xUnit | Kiosk service layer tests |
| `tests/CompileAndSip.KitchenDisplay.Tests` | xUnit | Kitchen display service layer tests |

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

## Run

The system requires three terminals — one per application:

**Terminal 1 — Order API** (must start first):

```bash
dotnet run --project src/CompileAndSip.OrderApi
```

**Terminal 2 — Kiosk TUI**:

```bash
dotnet run --project src/CompileAndSip.KioskTui
```

**Terminal 3 — Kitchen Display**:

```bash
dotnet run --project src/CompileAndSip.KitchenDisplay
```

Or use the convenience script to start all three:

```bash
./src/run-all.sh
```

## Configuration

- **Order API** listens on `http://localhost:5100` (configured in `CompileAndSip.OrderApi/Properties/launchSettings.json`).
- **TUI apps** read the Order API URL from `OrderApi:BaseUrl` in `appsettings.json` (default: `http://localhost:5100`).
