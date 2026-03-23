# Phase 7: Integration, ADRs & Documentation

Verify Aspire wiring, create ADRs for decisions made during the build, update documentation cross-references, and add "how to run" instructions.

## Read first

- `src/CompileAndSip.AppHost/Program.cs` — verify all 3 apps registered
- `docs/decisions/ADR-0001-solution-dotnet-platform.md` — existing ADR format (use as template)
- `docs/decisions/decisions.index.md` — current index
- `docs/solution/applications/kiosk-tui/kiosk-tui.index.md`
- `docs/solution/applications/order-api/order-api.index.md`
- `docs/solution/applications/kitchen-display/kitchen-display.index.md`
- `docs/business/scenarios/scenarios.index.md`

## Steps

### 1. Verify all tests pass

```bash
dotnet test --verbosity normal
```

All tests (domain, API, BDD, kiosk, kitchen display) must pass. Fix any failures first.

### 2. Verify Aspire AppHost

Confirm `src/CompileAndSip.AppHost/Program.cs` registers all 3 apps (`order-api`, `kiosk-tui`, `kitchen-display`) and that Kiosk/Kitchen use `.WithReference(orderApi)` for service discovery.

### 3. Create ADRs

Create 4 new ADRs in `docs/decisions/`. Follow the same format as ADR-0001 (Status, Context, Decision, Consequences).

**`ADR-0004-solution-project-structure.md`**
- Decision: Monorepo with shared Domain library, `.sln` at root, `src/` and `tests/` layout
- Alternatives: types in each project, separate Contracts library
- Consequences: single source of truth for domain types, no web dependency leaking into domain

**`ADR-0005-solution-test-strategy.md`**
- Decision: Three verification layers — BDD (Reqnroll at API boundary via WebApplicationFactory), TDD (xUnit + FluentAssertions for domain + API), unit tests for service layers
- Alternatives: BDD against full system, domain-level BDD only
- Consequences: BDD catches intent errors, TDD catches implementation errors, console rendering not tested

**`ADR-0006-solution-local-orchestration-aspire.md`**
- Decision: .NET Aspire AppHost for one-command start, service discovery, and observability dashboard
- Alternatives: shell script, Docker Compose, manual `dotnet run` × 3
- Consequences: one command to start all apps, built-in dashboard, no Docker needed, two extra projects (AppHost + ServiceDefaults)

**`ADR-0007-application-order-api-simulated-payment-gateway.md`**
- Decision: `IPaymentGateway` interface in Domain, `FakePaymentGateway` in API — configurable for success/decline/unavailable
- Alternatives: separate HTTP stub service, test doubles only
- Consequences: zero infrastructure, BDD can test all payment paths, clean boundary for future real gateway

### 4. Update decisions index

Update `docs/decisions/decisions.index.md` to list all 7 ADRs.

### 5. Update application landing pages

Add a "Source code" section to each application's index or context page pointing to the project path:

- `kiosk-tui.index.md` → `src/CompileAndSip.KioskTui/`
- `order-api.index.md` → `src/CompileAndSip.OrderApi/`
- `kitchen-display.index.md` → `src/CompileAndSip.KitchenDisplay/`
- Also reference `src/CompileAndSip.Domain/` as the shared domain library

### 6. Update scenario files with BDD test references

Add a "Verification" or "BDD test" reference to each scenario file:

- SCN-001 → `tests/CompileAndSip.Bdd.Tests/Features/SCN-001-browse-menu-select-drink.feature`
- SCN-002 → `tests/CompileAndSip.Bdd.Tests/Features/SCN-002-customise-drink.feature`
- SCN-003 → `tests/CompileAndSip.Bdd.Tests/Features/SCN-003-payment-succeeds.feature`
- SCN-004 → `tests/CompileAndSip.Bdd.Tests/Features/SCN-004-payment-fails.feature`
- SCN-005 → `tests/CompileAndSip.Bdd.Tests/Features/SCN-005-kitchen-marks-order-complete.feature`

### 7. Update root README.md

Add a "How to run" section:

```markdown
## How to run

**Prerequisites:** .NET 10 SDK (no Aspire workload needed — Aspire is NuGet-based)

**Start all apps:**
```bash
dotnet run --project src/CompileAndSip.AppHost
```

**Run all tests:**
```bash
dotnet test
```
```

### 8. Update phase status

Edit `work/002-system-build/README.md` — set all phases to "Complete".

## Verification

```bash
dotnet build
dotnet test
# All tests pass

ls docs/decisions/ADR-000{1,2,3,4,5,6,7}-*.md
# 7 files

dotnet test --verbosity normal 2>&1 | tail -5
# Expect 50+ total tests, zero failures
```

## Commit — do this now

You MUST commit to complete the build. Run these commands:

```bash
git add -A
git commit -m "feat: add ADRs, documentation cross-references, and how-to-run instructions"
```

Then update `work/002-system-build/README.md` — change Phase 7 status from "Not started" to "Complete".

All phases are now done.
