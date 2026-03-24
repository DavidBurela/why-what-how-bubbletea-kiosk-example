# Phase 1: Documentation

Enrich the portable BDD guidance doc and expand the project-specific BDD conventions with execution details.

Two files updated — one portable (reusable across projects), one project-specific (how this .NET project implements BDD).

## Context — read these files first

| # | File | What to extract |
|---|------|-----------------|
| 1 | `docs/docs.why-what-how.bdd.md` | Current version — thin initial draft to be rewritten |
| 2 | `docs/docs.why-what-how.md` | Parent framework doc — tone, style, and how BDD fits in the four-area model |
| 3 | `docs/docs.why-what-how.area-guidance.md` | Scenario expected sections (Journeys, Given/When/Then, Notes, Verification) |
| 4 | `docs/docs.conventions.md` | Linking direction rules, generated content markers, black-box vs white-box definitions |
| 5 | `docs/standards/testing/bdd-conventions.md` | Current version — to be expanded with execution details |
| 6 | `docs/standards/testing/testing-strategy.md` | Three-layer testing model for context |
| 7 | `docs/decisions/ADR-0005-solution-test-strategy.md` | Decision rationale for BDD at API boundary |
| 8 | `src/tests/CompileAndSip.Bdd.Tests/Support/Hooks.cs` | WebApplicationFactory pattern — reference for "How it works" |
| 9 | `src/tests/CompileAndSip.Bdd.Tests/Support/TestContext.cs` | Per-scenario state management — reference for "How it works" |
| 10 | `src/tests/CompileAndSip.Bdd.Tests/StepDefinitions/MenuSteps.cs` | Step definition pattern — thin HTTP adapter example |

## Steps

### 1. Rewrite `docs/docs.why-what-how.bdd.md`

This file is **PORTABLE** — it gets copied to other projects using the Why-What-How framework. It must be completely self-contained. No project-specific references (no JNY-001, no CompileAndSip, no `src/` paths, no specific drink names).

Replace the entire file with the following sections:

**Purpose** — expand beyond "Journeys capture narrative, Scenarios capture behaviour, BDD tests verify." Explain:
- BDD provides executable proof that business scenarios hold
- It bridges the gap between documented intent (what the business expects) and running software (what was actually built)
- Scenarios define expected behaviour in business language; BDD tests make that language executable
- When BDD tests pass, the team has concrete evidence that each scenario is satisfied — not just a human's opinion

**Behaviour stack** — keep the Journey → Scenario → BDD test hierarchy but flesh it out:
- Use a generic self-contained example: "A journey like 'Customer Places an Order' decomposes into scenarios — browsing available options, customising a selection, completing payment. Each scenario has one feature file containing one or more executable examples."
- Explain the 1:many relationships at each level
- A single flow may support multiple scenarios

**How BDD tests should execute** — NEW section, kept general and high-level:
- Tests should target the **service boundary** (the API), not internal source code. The service boundary is the same interface that real consumers use.
- Feature files use **business language only** — no HTTP verbs, status codes, JSON structures, or API paths. Write from the user's perspective.
- Step definitions are **thin adapters** that translate Gherkin into API calls. They contain no business logic.
- The test infrastructure should boot the **real application in-process** — no external process orchestration, no running instances to manage. Each scenario gets a fresh, isolated instance.
- Example approaches by stack (brief, not prescriptive):
  - .NET: `WebApplicationFactory<Program>` creates an in-memory test server with the full middleware pipeline
  - Java/Spring: `@SpringBootTest` with `TestRestTemplate` or `WebTestClient`
  - Node.js: `supertest` against the Express/Fastify app instance
- The pattern is the same regardless of stack: boot the real app, send real HTTP requests, assert on real responses — but without network overhead or process orchestration.

**Where things live** — refine the existing content with generic paths:
- Journeys: `docs/business/journeys/`
- Scenarios: `docs/business/scenarios/`
- BDD feature files: with the test code (not under `docs/`), placed under the relevant application test folder so they run in CI
- Verification reports: `docs/business/verification/` (generated artifacts — see Verification reporting section)

**What goes into each** — keep existing content (Journey, Scenario, BDD test guidance) but refine wording.

**Verification reporting** — NEW section explaining how to include verification status in the documentation structure:
- BDD test results can be surfaced as verification status within the documentation
- Recommend generating both a rich report (HTML — viewable locally or in CI artifacts) and a summary (markdown — renderable on platforms like GitHub)
- Place generated reports in `docs/business/verification/` — this is business-layer content (black-box verification of scenarios), not solution-layer
- The markdown summary should map scenario IDs to pass/fail status with a timestamp of the last run
- Mark generated sections with `<!-- BEGIN GENERATED -->` / `<!-- END GENERATED -->` markers so agents and humans know not to hand-edit them
- Link to verification status from individual scenario files and the business context or business index
- A generation script should be documented in the project's local development standards

**Non-blocking vs blocking** — keep existing content.

### 2. Expand `docs/standards/testing/bdd-conventions.md`

This file is **PROJECT-SPECIFIC** — it describes how this .NET project implements BDD. Add two new sections after the existing content.

**How it works** — explain the .NET test infrastructure:
- `WebApplicationFactory<Program>` (from `Microsoft.AspNetCore.Mvc.Testing`) boots the real ASP.NET Core application into an in-memory test server. No TCP port is opened; no external process is started.
- `CreateClient()` returns an `HttpClient` that sends real HTTP requests through the in-memory transport to the test server. The full middleware pipeline is exercised: routing, model binding, validation, serialization, error handling.
- Each scenario gets a **fresh** `WebApplicationFactory` instance via the `[BeforeScenario]` hook in `Support/Hooks.cs`. This guarantees scenario isolation — no shared state between scenarios.
- `TestContext` (in `Support/TestContext.cs`) holds per-scenario state: the `HttpClient`, the last `HttpResponseMessage`, and references to test doubles like `FakePaymentGateway`.
- `FakePaymentGateway` is resolved from the application's DI container via `_factory.Services.GetRequiredService<FakePaymentGateway>()`. This allows `Given` steps to configure payment outcomes (success, declined, unavailable) without modifying the application code.
- Step definitions (in `StepDefinitions/`) receive `TestContext` via Reqnroll's constructor injection. Each step method translates a Gherkin phrase into an HTTP call — for example, "When they view the menu" becomes `await _context.Client.GetAsync("/menu")`.

Include a simplified diagram in the explanation:

```
Feature File (business language)
  → Step Definitions (thin HTTP adapters)
    → HttpClient
      → In-Memory Test Server (WebApplicationFactory)
        → Real ASP.NET Core Pipeline (routing, validation, serialization)
          → Domain Logic + In-Memory Storage
```

**What this achieves** — explain the benefits and contrast with alternatives:
- **Business language preserved** — feature files read as plain English scenarios. Non-developers can review and validate them.
- **Tests at the real service boundary** — the same HTTP interface that real consumers (Kiosk TUI, Kitchen Display) use. If a route, DTO, or validation rule changes, BDD tests catch it.
- **Fast and deterministic** — no network latency, no port conflicts, no process startup wait. The full test suite runs in seconds.
- **Contrast with alternatives:**
  - Full E2E against running processes: realistic but slow, flaky (port conflicts, startup races), requires orchestration.
  - Domain-level testing only: fast but misses HTTP-layer issues (routing, serialization, model binding, status codes).
  - The `WebApplicationFactory` approach gives the coverage of E2E with the speed and reliability of unit tests.

## Verification

```bash
# docs.why-what-how.bdd.md must contain no project-specific references
grep -in 'JNY-001\|SCN-001\|CompileAndSip\|src/\|Compile & Sip\|bubble tea\|Kiosk\|OrderApi' docs/docs.why-what-how.bdd.md
# Should return zero matches

# bdd-conventions.md should reference the actual code files
grep -c 'Hooks.cs\|TestContext.cs\|WebApplicationFactory' docs/standards/testing/bdd-conventions.md
# Should return 3+ matches

# Existing tests still pass
cd src && dotnet test && cd ..
```

Then update `work/003-bdd-enhancements/README.md` — change Phase 1 status to "Complete".
