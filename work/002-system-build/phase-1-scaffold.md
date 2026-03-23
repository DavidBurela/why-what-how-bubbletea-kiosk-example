# Phase 1: Scaffold

Create the solution with 11 projects, wire Aspire, and verify it builds. No business logic — just the skeleton.

## Read first

- `docs/decisions/ADR-0001-solution-dotnet-platform.md` — .NET platform, Spectre.Console TUI, ASP.NET Core API
- `docs/solution/solution-context.md` — 3 apps, data flow, system boundary

## Steps

### 1. Clean stale directories

Prior attempts left empty directories with stale `obj/` folders. Remove them:

```bash
find src tests -name "obj" -type d -exec rm -rf {} + 2>/dev/null; true
```

### 2. Create solution and all projects

```bash
dotnet new sln -n CompileAndSip

# Source projects
dotnet new classlib -o src/CompileAndSip.Domain
dotnet new web -o src/CompileAndSip.OrderApi
dotnet new console -o src/CompileAndSip.KioskTui
dotnet new console -o src/CompileAndSip.KitchenDisplay
dotnet new console -o src/CompileAndSip.AppHost
dotnet new classlib -o src/CompileAndSip.ServiceDefaults

# Test projects
dotnet new xunit -o tests/CompileAndSip.Domain.Tests
dotnet new xunit -o tests/CompileAndSip.OrderApi.Tests
dotnet new xunit -o tests/CompileAndSip.Bdd.Tests
dotnet new xunit -o tests/CompileAndSip.KioskTui.Tests
dotnet new xunit -o tests/CompileAndSip.KitchenDisplay.Tests

# Add all to solution
dotnet sln add src/**/*.csproj tests/**/*.csproj
```

### 3. Remove all placeholder files

```bash
find src -name "Class1.cs" -delete
find tests -name "UnitTest1.cs" -delete
# Also remove any auto-generated test files
find tests -name "Test1.cs" -delete
```

### 4. Add project references

```bash
# Apps reference Domain + ServiceDefaults
dotnet add src/CompileAndSip.OrderApi reference src/CompileAndSip.Domain src/CompileAndSip.ServiceDefaults
dotnet add src/CompileAndSip.KioskTui reference src/CompileAndSip.Domain src/CompileAndSip.ServiceDefaults
dotnet add src/CompileAndSip.KitchenDisplay reference src/CompileAndSip.Domain src/CompileAndSip.ServiceDefaults

# AppHost references all runnable apps
dotnet add src/CompileAndSip.AppHost reference src/CompileAndSip.OrderApi src/CompileAndSip.KioskTui src/CompileAndSip.KitchenDisplay

# Test projects reference their targets
dotnet add tests/CompileAndSip.Domain.Tests reference src/CompileAndSip.Domain
dotnet add tests/CompileAndSip.OrderApi.Tests reference src/CompileAndSip.OrderApi
dotnet add tests/CompileAndSip.Bdd.Tests reference src/CompileAndSip.OrderApi
dotnet add tests/CompileAndSip.KioskTui.Tests reference src/CompileAndSip.KioskTui
dotnet add tests/CompileAndSip.KitchenDisplay.Tests reference src/CompileAndSip.KitchenDisplay
```

### 5. Add NuGet packages

```bash
# Aspire hosting
dotnet add src/CompileAndSip.AppHost package Aspire.Hosting.AppHost

# ServiceDefaults
dotnet add src/CompileAndSip.ServiceDefaults package Microsoft.Extensions.Http.Resilience
dotnet add src/CompileAndSip.ServiceDefaults package Microsoft.Extensions.ServiceDiscovery
dotnet add src/CompileAndSip.ServiceDefaults package OpenTelemetry.Exporter.OpenTelemetryProtocol
dotnet add src/CompileAndSip.ServiceDefaults package OpenTelemetry.Extensions.Hosting
dotnet add src/CompileAndSip.ServiceDefaults package OpenTelemetry.Instrumentation.AspNetCore
dotnet add src/CompileAndSip.ServiceDefaults package OpenTelemetry.Instrumentation.Http
dotnet add src/CompileAndSip.ServiceDefaults package OpenTelemetry.Instrumentation.Runtime

# TUI libraries
dotnet add src/CompileAndSip.KioskTui package Spectre.Console
dotnet add src/CompileAndSip.KitchenDisplay package Spectre.Console

# Testing packages
dotnet add tests/CompileAndSip.Domain.Tests package FluentAssertions
dotnet add tests/CompileAndSip.OrderApi.Tests package FluentAssertions
dotnet add tests/CompileAndSip.OrderApi.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet add tests/CompileAndSip.KioskTui.Tests package FluentAssertions
dotnet add tests/CompileAndSip.KitchenDisplay.Tests package FluentAssertions
dotnet add tests/CompileAndSip.Bdd.Tests package FluentAssertions
dotnet add tests/CompileAndSip.Bdd.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet add tests/CompileAndSip.Bdd.Tests package Reqnroll
dotnet add tests/CompileAndSip.Bdd.Tests package Reqnroll.xUnit
```

### 6. Configure Aspire AppHost

**Aspire is NuGet-based in .NET 10. No workload, no templates.**

Edit `src/CompileAndSip.AppHost/CompileAndSip.AppHost.csproj` to add the Aspire SDK and `IsAspireHost` property. Check NuGet for the latest `Aspire.AppHost.Sdk` version:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <Sdk Name="Aspire.AppHost.Sdk" Version="<latest>" />

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <IsAspireHost>true</IsAspireHost>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Aspire.Hosting.AppHost" />
  </ItemGroup>

  <!-- Project references added by dotnet add reference -->
</Project>
```

Write `src/CompileAndSip.AppHost/Program.cs`:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var orderApi = builder.AddProject<Projects.CompileAndSip_OrderApi>("order-api");

builder.AddProject<Projects.CompileAndSip_KioskTui>("kiosk-tui")
    .WithReference(orderApi)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.CompileAndSip_KitchenDisplay>("kitchen-display")
    .WithReference(orderApi)
    .WithExternalHttpEndpoints();

builder.Build().Run();
```

### 7. Create ServiceDefaults

Write `src/CompileAndSip.ServiceDefaults/Extensions.cs` with two extension methods:

- `AddServiceDefaults(this IHostApplicationBuilder)` — configures OpenTelemetry (logging, metrics, tracing), health checks, service discovery, and default HTTP client resilience/discovery.
- `MapDefaultEndpoints(this WebApplication)` — maps `/health` and `/alive` health check endpoints.

This is the standard Aspire ServiceDefaults pattern. Reference the Aspire documentation or samples for the exact implementation.

### 8. Write stub Program.cs for each app

**OrderApi** `src/CompileAndSip.OrderApi/Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
var app = builder.Build();
app.MapDefaultEndpoints();
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

### 9. Create standards files

Standards capture cross-cutting conventions so future agent sessions maintain consistency. Create the following files under `docs/standards/`. These distil decisions already made in the existing ADRs — they are not new decisions.

**`docs/standards/standards.index.md`** — navigation page linking to Engineering, Testing, and Delivery sub-areas.

**`docs/standards/engineering/project-structure.md`** — from ADR-0001 and the solution structure above:
- `.sln` at repo root, source in `src/`, tests in `tests/`
- `CompileAndSip.<Name>` naming convention
- Dependency rules: Domain has zero external deps, apps reference Domain + ServiceDefaults, test projects mirror source projects
- AppHost references all runnable apps

**`docs/standards/engineering/coding-conventions.md`** — from ADR-0001, ADR-0002:
- .NET 10, file-scoped namespaces
- Records for immutable value types, enums for fixed option sets
- Sealed classes where inheritance is not needed
- Minimal API (not controllers)
- Interface-based DI for testability
- `ConcurrentDictionary` for in-memory storage, `Interlocked.Increment` for atomic counters

**`docs/standards/delivery/local-development.md`** — from ADR-0001 and Aspire setup:
- `dotnet run --project src/CompileAndSip.AppHost` starts all apps
- `dotnet test` runs all test layers
- No Aspire workload needed — Aspire is NuGet-based
- No Docker required
- Aspire dashboard provides observability

### 10. Verify build

```bash
dotnet build
dotnet test   # Should pass with 0 tests (no test files)
dotnet sln list   # Should show 11 projects
find docs/standards -name '*.md' | wc -l   # Should be 4 files
```

Fix any errors before proceeding.

## Commit — do this now before moving on

You MUST commit before proceeding to the next phase. Run these commands:

```bash
git add -A
git commit -m "feat: scaffold solution with 11 projects, Aspire, and engineering standards"
```

Then update `work/002-system-build/README.md` — change Phase 1 status from "Not started" to "Complete".

Do NOT start Phase 2 until this commit succeeds.
