# System Build — Compile & Sip

Build the Compile & Sip kiosk system: three .NET 10 applications orchestrated by Aspire, verified with TDD and BDD.

## Autopilot prompt

Copy this into a new agent session:

```text
Execute the build plan at `work/002-system-build/README.md`.

For EACH phase (1 through 7), in order:
1. Read the phase file (e.g. `work/002-system-build/phase-1-scaffold.md`)
2. Read the context files listed at the top of the phase
3. Execute the numbered steps in order
4. For TDD phases (2, 3): write the failing test FIRST, then implement to pass, then refactor
5. Run the verification commands — fix any failure before continuing
6. Commit with the message specified at the bottom
7. Edit `work/002-system-build/README.md` — change the phase status to "Complete"

Rules:
- Read ONE phase file at a time. Complete it before reading the next.
- If `dotnet build` or `dotnet test` fails, fix it. Never skip a broken build.
- Keep code simple. This is a reference example, not production.
- File-scoped namespaces, records for immutable types, sealed classes.
- Aspire is NuGet-based — no workload install, no Aspire templates.
```

## Solution structure

```
CompileAndSip.sln
src/
  CompileAndSip.AppHost/           # Aspire orchestrator
  CompileAndSip.ServiceDefaults/   # Shared OpenTelemetry, health, service discovery
  CompileAndSip.Domain/            # Shared domain model (no web dependencies)
  CompileAndSip.OrderApi/          # ASP.NET Core Minimal API
  CompileAndSip.KioskTui/          # Spectre.Console customer terminal
  CompileAndSip.KitchenDisplay/    # Spectre.Console kitchen polling display
tests/
  CompileAndSip.Domain.Tests/
  CompileAndSip.OrderApi.Tests/
  CompileAndSip.Bdd.Tests/
  CompileAndSip.KioskTui.Tests/
  CompileAndSip.KitchenDisplay.Tests/
```

## Technology stack

| Concern | Choice | Reference |
|---------|--------|-----------|
| Platform | .NET 10 | ADR-0001 |
| API | ASP.NET Core Minimal API | ADR-0001 |
| TUI | Spectre.Console | ADR-0001 |
| Storage | In-memory (ConcurrentDictionary) | ADR-0002 |
| Communication | REST/HTTP polling | ADR-0003 |
| Orchestration | .NET Aspire AppHost | Phase 1 |
| TDD | xUnit + FluentAssertions | Phase 2 |
| BDD | Reqnroll + xUnit | Phase 4 |
| Payment | In-process fake (IPaymentGateway) | Phase 3 |

## Phases

| # | File | What it produces | Standards created | Status |
|---|------|-----------------|-------------------|--------|
| 1 | `phase-1-scaffold.md` | Solution skeleton — 11 projects, Aspire, builds clean | `project-structure`, `coding-conventions`, `local-development` | Not started |
| 2 | `phase-2-domain-model.md` | Domain types + ~20 TDD tests | `testing-strategy`, `tdd-conventions` | Not started |
| 3 | `phase-3-order-api.md` | 4 API endpoints + ~20 integration tests | `api-design` | Not started |
| 4 | `phase-4-bdd-tests.md` | BDD features for SCN-001–005 (~12 scenarios) | `bdd-conventions` | Not started |
| 5 | `phase-5-kiosk-tui.md` | Customer terminal app + service tests | — | Not started |
| 6 | `phase-6-kitchen-display.md` | Kitchen polling display + service tests | — | Not started |
| 7 | `phase-7-integration.md` | ADRs, doc updates, final verification | — | Not started |

| Phase | Commit message |
|-------|---------------|
| 1 | `feat: scaffold solution structure with Aspire, standards, and ADRs` |
| 2 | `feat: implement domain model with TDD (drinks, orders, pricing)` |
| 3 | `feat: implement Order API endpoints with TDD` |
| 4 | `feat: add BDD verification for all business scenarios` |
| 5 | `feat: implement Kiosk TUI with Spectre.Console` |
| 6 | `feat: implement Kitchen Display with polling` |
| 7 | `feat: wire integration, polish, and update documentation` |

## New ADRs (created in Phase 1)

| ADR | Scope | Title |
|-----|-------|-------|
| ADR-0004 | Solution | Project Structure — Monorepo with Shared Domain Library |
| ADR-0005 | Solution | Test Strategy — TDD + BDD at API Boundary |
| ADR-0006 | Solution | Local Orchestration via .NET Aspire |
| ADR-0007 | Application — Order API | Simulated Payment Gateway via Interface |

## Standards (created in Phase 1)

```
docs/standards/
  standards.index.md
  engineering/
    project-structure.md
    coding-conventions.md
    api-design.md
  testing/
    testing-strategy.md
    tdd-conventions.md
    bdd-conventions.md
  delivery/
    local-development.md
```

## Decisions

| # | Decision | Rationale |
|---|----------|-----------|
| D1 | BDD at API boundary, TDD for domain and app logic | All business logic lives in Order API + Domain. API boundary IS the business boundary. BDD feature files read as pure business language. TDD covers internals. |
| D2 | Shared Domain library | Avoids type duplication across API, TUI, and KDS. Single source of truth for drinks, orders, pricing. |
| D3 | Aspire AppHost for orchestration | Solves "start/stop together" with one command. Free dashboard and OpenTelemetry. No Docker required. |
| D4 | In-process payment fake via interface | Simplest approach — `IPaymentGateway` with `FakePaymentGateway` configurable for success/fail/unavailable. |
| D5 | Spectre.Console for both TUI apps | Kiosk uses interactive prompts, Kitchen Display uses tables/live display. Consistent UX layer. |
| D6 | Phases 5 and 6 parallelizable | Both depend on API (Phase 3), not each other. |
| D7 | One .feature file per SCN-xxx scenario | Clear traceability from business scenario to BDD verification. |
| D8 | `.sln` at repo root, source in `src/`, tests in `tests/` | Standard .NET repo layout (matches Microsoft repos). Solution file at root references both directories. Keeps root clean — only `.sln`, `.gitignore`, docs, and config at top level. |
| D9 | Semantic commits after each verified phase | Enables rollback to earlier phases if plans need adjustment. |
| D10 | `.gitignore` from `dotnet new gitignore` | Prevents bin/, obj/, publish outputs, and user settings from being committed. Added in Phase 1. |
