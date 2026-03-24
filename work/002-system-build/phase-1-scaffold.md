# Phase 1: Scaffold

Create the solution with 7 projects inside `src/`, verify it builds. No business logic — just the skeleton.

## Read first

- `docs/decisions/ADR-0001-solution-dotnet-platform.md` — .NET platform, Spectre.Console TUI, ASP.NET Core API
- `docs/solution/solution-context.md` — 3 apps, data flow, system boundary

## Steps

### 1. Clean stale directories

Prior attempts may have left empty directories with stale `obj/` folders. Remove them:

```bash
find src tests -name "obj" -type d -exec rm -rf {} + 2>/dev/null; true
rm -rf src/ tests/ CompileAndSip.sln CompileAndSip.slnx 2>/dev/null; true
```

### 2. Create solution and all projects

All projects live under `src/`. Run commands from the repository root:

```bash
mkdir -p src
cd src

# Create solution file
dotnet new sln -n CompileAndSip

# Source projects
dotnet new web -o CompileAndSip.OrderApi
dotnet new console -o CompileAndSip.KioskTui
dotnet new console -o CompileAndSip.KitchenDisplay

# Test projects (under src/tests/)
dotnet new xunit -o tests/CompileAndSip.OrderApi.Tests
dotnet new xunit -o tests/CompileAndSip.Bdd.Tests
dotnet new xunit -o tests/CompileAndSip.KioskTui.Tests
dotnet new xunit -o tests/CompileAndSip.KitchenDisplay.Tests

# Add all to solution
dotnet sln add \
  CompileAndSip.OrderApi \
  CompileAndSip.KioskTui \
  CompileAndSip.KitchenDisplay \
  tests/CompileAndSip.OrderApi.Tests \
  tests/CompileAndSip.Bdd.Tests \
  tests/CompileAndSip.KioskTui.Tests \
  tests/CompileAndSip.KitchenDisplay.Tests

cd ..
```

### 3. Remove all placeholder files

```bash
find src -name "Class1.cs" -delete
find src -name "UnitTest1.cs" -delete
find src -name "Test1.cs" -delete
```

### 4. Add project references

```bash
cd src

# Test projects reference their targets
dotnet add tests/CompileAndSip.OrderApi.Tests reference CompileAndSip.OrderApi
dotnet add tests/CompileAndSip.Bdd.Tests reference CompileAndSip.OrderApi
dotnet add tests/CompileAndSip.KioskTui.Tests reference CompileAndSip.KioskTui
dotnet add tests/CompileAndSip.KitchenDisplay.Tests reference CompileAndSip.KitchenDisplay

cd ..
```

Note: the three source projects have **no references to each other**. TUI apps communicate with the Order API via HTTP only — matching the architecture diagram exactly.

### 5. Add NuGet packages

```bash
cd src

# TUI libraries
dotnet add CompileAndSip.KioskTui package Spectre.Console
dotnet add CompileAndSip.KitchenDisplay package Spectre.Console

# Testing packages
dotnet add tests/CompileAndSip.OrderApi.Tests package FluentAssertions
dotnet add tests/CompileAndSip.OrderApi.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet add tests/CompileAndSip.KioskTui.Tests package FluentAssertions
dotnet add tests/CompileAndSip.KitchenDisplay.Tests package FluentAssertions
dotnet add tests/CompileAndSip.Bdd.Tests package FluentAssertions
dotnet add tests/CompileAndSip.Bdd.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet add tests/CompileAndSip.Bdd.Tests package Reqnroll
dotnet add tests/CompileAndSip.Bdd.Tests package Reqnroll.xUnit

cd ..
```

### 6. Configure OrderApi launch settings

Create `src/CompileAndSip.OrderApi/Properties/launchSettings.json` — bind to port 5100 so TUI apps have a known address:

```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5100",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### 7. Write stub Program.cs for each app

**OrderApi** `src/CompileAndSip.OrderApi/Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Compile & Sip Order API");

app.Run();

public partial class Program { } // Enables WebApplicationFactory in tests
```

**KioskTui** `src/CompileAndSip.KioskTui/Program.cs`:

```csharp
using Spectre.Console;

AnsiConsole.MarkupLine("[bold green]Compile & Sip — Kiosk[/]");
AnsiConsole.MarkupLine("(Kiosk TUI will be implemented in Phase 5)");
```

**KitchenDisplay** `src/CompileAndSip.KitchenDisplay/Program.cs`:

```csharp
using Spectre.Console;

AnsiConsole.MarkupLine("[bold blue]Compile & Sip — Kitchen Display[/]");
AnsiConsole.MarkupLine("(Kitchen Display will be implemented in Phase 6)");
```

### 8. Create `src/README.md`

Create `src/README.md` covering:

- **Project layout** — table of 3 source projects + 4 test projects with one-line descriptions
- **Build** — `cd src && dotnet build`
- **Test** — `dotnet test`
- **Run** — 3-terminal instructions: Terminal 1 starts OrderApi on port 5100, Terminal 2 starts KioskTui, Terminal 3 starts KitchenDisplay. Mention the `run-all.sh` convenience script.
- **Configuration** — OrderApi listens on `http://localhost:5100` (via launchSettings.json). TUI apps read the Order API URL from `OrderApi:BaseUrl` in appsettings.json (default: `http://localhost:5100`).

### 9. Create `src/run-all.sh`

Create `src/run-all.sh` — a bash script that starts all 3 apps:

```bash
#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "Starting Order API on http://localhost:5100..."
dotnet run --project "$SCRIPT_DIR/CompileAndSip.OrderApi" &
API_PID=$!
sleep 3

echo "Starting Kiosk TUI..."
dotnet run --project "$SCRIPT_DIR/CompileAndSip.KioskTui" &
KIOSK_PID=$!

echo "Starting Kitchen Display..."
dotnet run --project "$SCRIPT_DIR/CompileAndSip.KitchenDisplay" &
KDS_PID=$!

echo ""
echo "All apps started. Press Ctrl+C to stop."
trap "kill $API_PID $KIOSK_PID $KDS_PID 2>/dev/null" EXIT
wait
```

Make it executable: `chmod +x src/run-all.sh`

### 10. Create standards files

Standards capture cross-cutting conventions so future agent sessions maintain consistency. Create the following files under `docs/standards/`. These distil decisions already made — they are not new decisions.

**`docs/standards/standards.index.md`** — navigation page linking to Engineering, Testing, and Delivery sub-areas.

**`docs/standards/engineering/project-structure.md`** — from the solution structure above:
- Solution file in `src/`, source projects alongside it, tests in `src/tests/`
- `CompileAndSip.<Name>` naming convention
- Three source projects — one per documented application — with no inter-project references
- Dependency rules: test projects reference their corresponding source project. TUI apps do NOT reference OrderApi — they communicate via HTTP only.
- Each source project folder maps 1:1 to a box on the C4 container diagram

**`docs/standards/engineering/coding-conventions.md`** — from ADR-0001, ADR-0002:
- .NET 10, file-scoped namespaces
- Records for immutable value types, enums for fixed option sets
- Sealed classes where inheritance is not needed
- Minimal API (not controllers)
- Interface-based DI for testability
- `ConcurrentDictionary` for in-memory storage, `Interlocked.Increment` for atomic counters

**`docs/standards/delivery/local-development.md`** — from the run instructions:
- Each app starts independently with `dotnet run --project src/CompileAndSip.<Name>`
- OrderApi listens on `http://localhost:5100` (configured in launchSettings.json)
- TUI apps connect to Order API via `OrderApi:BaseUrl` config (default `http://localhost:5100`)
- `cd src && dotnet test` runs all test layers
- Convenience script: `./src/run-all.sh` starts all 3 apps

### 11. Verify build

```bash
cd src
dotnet build
dotnet test   # Should pass with 0 tests (no test files yet)
dotnet sln list   # Should show 7 projects
cd ..
find docs/standards -name '*.md' | wc -l   # Should be 4 files
```

Fix any errors before proceeding.

## Commit — do this now before moving on

You MUST commit before proceeding to the next phase. Run these commands:

```bash
git add -A
git commit -m "feat(phase-1): scaffold solution structure with standards"
```

Then update `work/002-system-build/README.md` — change Phase 1 status from "Not started" to "Complete".

Do NOT start Phase 2 until this commit succeeds.
