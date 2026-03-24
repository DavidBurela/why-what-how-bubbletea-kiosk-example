# Phase 7: Integration, ADRs & Documentation

Verify the build, create ADRs for decisions made during the build, update documentation cross-references, and finalize the root README.

## Read first

- `docs/decisions/ADR-0001-solution-dotnet-platform.md` — existing ADR format (use as template)
- `docs/decisions/decisions.index.md` — current index
- `docs/solution/applications/kiosk-tui/kiosk-tui.index.md`
- `docs/solution/applications/order-api/order-api.index.md`
- `docs/solution/applications/kitchen-display/kitchen-display.index.md`
- `docs/business/scenarios/scenarios.index.md`

## Steps

### 1. Verify all tests pass

```bash
cd src
dotnet test --verbosity normal
```

All tests (domain, API, BDD, kiosk, kitchen display) must pass. Fix any failures first.

### 2. Create ADRs

Create 4 new ADRs in `docs/decisions/`. Follow the same format as ADR-0001 (Status, Context, Decision, Consequences).

**`ADR-0004-solution-project-structure.md`**
- Decision: Three source projects — one per documented application — with the solution file and tests inside `src/`. No shared libraries. TUI apps communicate with the Order API over HTTP only, using their own DTO types.
- Alternatives: shared Domain library (single source of truth but hidden coupling), types duplicated in each project (no shared dependency but maintenance burden), Contracts NuGet package (versioning overhead)
- Consequences: project count matches the C4 container diagram exactly (3 apps). No hidden coupling. TUI apps can be understood in isolation. Trade-off: DTOs are duplicated across TUI apps and the API, but they're small and the HTTP boundary is the authoritative contract.

**`ADR-0005-solution-test-strategy.md`**
- Decision: Four test projects across two verification layers — BDD (Reqnroll at API boundary via WebApplicationFactory) and TDD (xUnit + FluentAssertions for domain, API, and service layers). Console rendering is not tested.
- Alternatives: BDD against full running system, domain-level BDD only, single test project for everything
- Consequences: BDD catches intent errors using business language. TDD catches implementation errors. Each test project mirrors or tests one source project. No flaky UI-dependent tests.

**`ADR-0006-solution-manual-orchestration.md`**
- Decision: Applications start independently with `dotnet run`. No .NET Aspire AppHost. A convenience script (`run-all.sh`) starts all three apps. OrderApi binds to port 5100 via launchSettings; TUI apps read the URL from configuration.
- Alternatives: .NET Aspire AppHost (one-command start, dashboard, OpenTelemetry), Docker Compose, shell orchestration with process monitoring
- Consequences: Zero additional projects beyond the three applications. Project structure maps 1:1 to the architecture diagram. No Aspire SDK, OpenTelemetry, or service discovery dependencies. Trade-off: no built-in dashboard or distributed tracing. Acceptable for a reference example where the documentation framework is the focus, not operational observability.

**`ADR-0007-application-order-api-simulated-payment-gateway.md`**
- Decision: `IPaymentGateway` interface and `FakePaymentGateway` both live in the OrderApi project. The fake is configurable for success/decline/unavailable outcomes. BDD tests configure it via DI to verify all payment paths.
- Alternatives: separate HTTP stub service, test doubles only in test project
- Consequences: zero infrastructure for payment simulation. BDD can test success, decline, and unavailable scenarios. Clean interface boundary for a future real gateway. The interface is internal to OrderApi — TUI apps see only HTTP responses.

### 3. Update decisions index

Update `docs/decisions/decisions.index.md` to list all 7 ADRs.

### 4. Update application landing pages

Add a "Source code" section to each application's index or context page pointing to the project path:

- `kiosk-tui.index.md` → `src/CompileAndSip.KioskTui/`
- `order-api.index.md` → `src/CompileAndSip.OrderApi/`
- `kitchen-display.index.md` → `src/CompileAndSip.KitchenDisplay/`

### 5. Update scenario files with BDD test references

Add a "Verification" or "BDD test" reference to each scenario file:

- SCN-001 → `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-001-browse-menu-select-drink.feature`
- SCN-002 → `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-002-customise-drink.feature`
- SCN-003 → `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-003-payment-succeeds.feature`
- SCN-004 → `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-004-payment-fails.feature`
- SCN-005 → `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-005-kitchen-marks-order-complete.feature`

### 6. Update root README.md

Add a "How to run" section pointing to `src/README.md` for full instructions:

```markdown
## How to run

**Prerequisites:** .NET 10 SDK

**Quick start:**

    cd src
    dotnet build
    dotnet test

See [src/README.md](src/README.md) for full instructions including how to start all three applications.
```

### 7. Update phase status

Edit `work/002-system-build/README.md` — set all phases to "Complete".

## Verification

```bash
cd src
dotnet build
dotnet test
# All tests pass
cd ..

ls docs/decisions/ADR-000{1,2,3,4,5,6,7}-*.md
# 7 files

cd src && dotnet test --verbosity normal 2>&1 | tail -5
# Expect 50+ total tests, zero failures
cd ..
```

## Commit — do this now

You MUST commit to complete the build. Run these commands:

```bash
git add -A
git commit -m "feat(phase-7): finalize ADRs, documentation, and verification"
```

Then update `work/002-system-build/README.md` — change Phase 7 status from "Not started" to "Complete".

All phases are now done.
