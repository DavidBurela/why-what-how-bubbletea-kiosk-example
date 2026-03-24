# System Build — Compile & Sip

Build the Compile & Sip kiosk system: three .NET 10 applications, verified with TDD and BDD.

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
6. Commit with the message specified at the bottom of the phase file
7. Edit `work/002-system-build/README.md` — change the phase status to "Complete"

Rules:
- Read ONE phase file at a time. Complete it before reading the next.
- If `dotnet build` or `dotnet test` fails, fix it. Never skip a broken build.
- Keep code simple. This is a reference example, not production.
- File-scoped namespaces, records for immutable types, sealed classes.
- All commands that reference the solution or projects run from the `src/` directory.
```

## Solution structure

All source code and the solution file live inside `src/`:

```
src/
  CompileAndSip.sln                 # Solution file
  README.md                         # Project layout, build, test, run instructions
  run-all.sh                        # Convenience script to start all 3 apps
  CompileAndSip.OrderApi/           # ASP.NET Core Minimal API + domain logic
  CompileAndSip.KioskTui/           # Spectre.Console customer terminal
  CompileAndSip.KitchenDisplay/     # Spectre.Console kitchen polling display
  tests/
    CompileAndSip.OrderApi.Tests/   # Domain TDD + API integration tests
    CompileAndSip.Bdd.Tests/        # Reqnroll BDD features (SCN-001–005)
    CompileAndSip.KioskTui.Tests/   # Kiosk service layer tests
    CompileAndSip.KitchenDisplay.Tests/  # Kitchen display service layer tests
```

Three source projects — one per documented application. Four test projects.

## Technology stack

| Concern | Choice | Reference |
|---------|--------|-----------|
| Platform | .NET 10 | ADR-0001 |
| API | ASP.NET Core Minimal API | ADR-0001 |
| TUI | Spectre.Console | ADR-0001 |
| Storage | In-memory (ConcurrentDictionary) | ADR-0002 |
| Communication | REST/HTTP polling | ADR-0003 |
| TDD | xUnit + FluentAssertions | Phase 2 |
| BDD | Reqnroll + xUnit | Phase 4 |
| Payment | In-process fake (IPaymentGateway) | Phase 3 |

## Phases

| # | File | What it produces | Standards created | Status |
|---|------|-----------------|-------------------|--------|
| 1 | `phase-1-scaffold.md` | Solution skeleton — 7 projects, builds clean | `project-structure`, `coding-conventions`, `local-development` | Complete |
| 2 | `phase-2-domain-model.md` | Domain types + ~20 TDD tests (in OrderApi) | `testing-strategy`, `tdd-conventions` | Complete |
| 3 | `phase-3-order-api.md` | 4 API endpoints + ~20 integration tests | `api-design` | Complete |
| 4 | `phase-4-bdd-tests.md` | BDD features for SCN-001–005 (~12 scenarios) | `bdd-conventions` | Complete |
| 5 | `phase-5-kiosk-tui.md` | Customer terminal app + service tests | — | Not started |
| 6 | `phase-6-kitchen-display.md` | Kitchen polling display + service tests | — | Not started |
| 7 | `phase-7-integration.md` | ADRs, doc updates, final verification | — | Not started |

| Phase | Commit message |
|-------|---------------|
| 1 | `feat(phase-1): scaffold solution structure with standards` |
| 2 | `feat(phase-2): implement domain model with TDD` |
| 3 | `feat(phase-3): implement Order API endpoints with TDD` |
| 4 | `feat(phase-4): add BDD verification for all business scenarios` |
| 5 | `feat(phase-5): implement Kiosk TUI with Spectre.Console` |
| 6 | `feat(phase-6): implement Kitchen Display with polling` |
| 7 | `feat(phase-7): finalize ADRs, documentation, and verification` |

## New ADRs (created in Phase 7)

| ADR | Scope | Title |
|-----|-------|-------|
| ADR-0004 | Solution | Project Structure — Three Apps with Co-located Tests |
| ADR-0005 | Solution | Test Strategy — TDD + BDD at API Boundary |
| ADR-0006 | Solution | Manual Orchestration over Aspire |
| ADR-0007 | Application — Order API | Simulated Payment Gateway via Interface |

## Standards (created across Phases 1–4)

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
| D1 | BDD at API boundary, TDD for domain and app logic | All business logic lives in Order API. API boundary IS the business boundary. BDD feature files read as pure business language. TDD covers internals. |
| D2 | Domain types live inside Order API — no shared library | TUI apps communicate via HTTP with their own DTOs. The API is the domain owner. Shared libraries create coupling that the architecture diagram doesn't show. |
| D3 | No Aspire — apps run standalone | Each app starts independently with `dotnet run`. A convenience script starts all three. This keeps the project structure 1:1 with the C4 container diagram. |
| D4 | In-process payment fake via interface | Simplest approach — `IPaymentGateway` with `FakePaymentGateway` configurable for success/fail/unavailable. |
| D5 | Spectre.Console for both TUI apps | Kiosk uses interactive prompts, Kitchen Display uses tables/live display. Consistent UX layer. |
| D6 | Phases 5 and 6 parallelizable | Both depend on API (Phase 3), not each other. |
| D7 | One .feature file per SCN-xxx scenario | Clear traceability from business scenario to BDD verification. |
| D8 | Solution file in `src/`, tests in `src/tests/` | Keeps the implementation self-contained. Readers can ignore `src/` to focus on documentation, or ignore everything else to focus on code. |
| D9 | Semantic commits with phase tag after each verified phase | `feat(phase-N): description` enables rollback to any phase boundary. |
| D10 | `.gitignore` from `dotnet new gitignore` | Prevents bin/, obj/, publish outputs, and user settings from being committed. Added in Phase 1. |
