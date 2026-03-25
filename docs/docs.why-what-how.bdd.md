# Extension: Journeys, Scenarios, and BDD

This document explains how business behaviour is defined and verified using Behaviour-Driven Development (BDD).

## Purpose

BDD provides executable proof that business scenarios hold. It bridges the gap between documented intent — what the business expects — and running software — what was actually built.

- **Scenarios** define expected behaviour in business language.
- **BDD tests** make that language executable.
- **When BDD tests pass**, the team has concrete evidence that each scenario is satisfied — not just a human's opinion, but automated verification that runs on demand.

This eliminates ambiguity about whether requirements are met. Instead of relying on manual review or informal sign-off, the team can point to a passing test suite and say: "every documented scenario has been verified against the real system."

## Behaviour stack

Business behaviour is captured as a hierarchy:

```
Journey (narrative)
  → Scenario (behavioural slice)
    → BDD test (executable example)
```

**Journeys** are end-to-end user narratives. A journey like "Customer Places an Order" describes the full arc — browsing available options, customising a selection, completing payment, and receiving confirmation. Journeys are cross-application and outcome-focused.

**Scenarios** are behavioural slices extracted from journeys. Each scenario isolates one observable behaviour that must hold. A single journey typically decomposes into multiple scenarios — one for browsing, one for customisation, one for payment success, one for payment failure, and so on.

**BDD tests** are executable examples of a scenario. Each scenario has one feature file containing one or more concrete examples expressed as Given/When/Then. A scenario with edge cases or role variations may have several examples within one feature file.

Key relationships:

- A journey usually requires multiple scenarios.
- A scenario often requires multiple BDD test examples (variants, edge cases, roles).
- A single flow may support multiple scenarios.
- Mappings are not necessarily 1:1 at any level.

## How BDD tests should execute

BDD tests should target the **service boundary** — the same interface that real consumers use (typically an API). This means tests exercise the real application through its public contract, not through internal classes or mocked layers.

### Feature files use business language only

Feature files are written from the user's perspective. No HTTP verbs, status codes, JSON structures, or API paths. A non-technical stakeholder should be able to read a feature file and confirm it matches their expectation.

### Step definitions are thin adapters

Step definitions translate Gherkin phrases into API calls. They contain no business logic — their only job is to map human-readable steps to HTTP requests and assert on responses.

### Boot the real application in-process

The test infrastructure should boot the real application in-process — no external process orchestration, no running instances to manage, no TCP ports to allocate. Each scenario gets a fresh, isolated instance of the application.

Example approaches by stack:

- **.NET** — `WebApplicationFactory<Program>` creates an in-memory test server with the full middleware pipeline.
- **Java/Spring** — `@SpringBootTest` with `TestRestTemplate` or `WebTestClient`.
- **Node.js** — `supertest` against the Express or Fastify app instance.

The pattern is the same regardless of stack: boot the real app, send real HTTP requests, assert on real responses — but without network overhead or process orchestration.

## Where things live

Documentation:

- **Journeys:** `docs/business/journeys/`
- **Scenarios:** `docs/business/scenarios/`

Tests:

- **BDD feature files** live with the test code, not under `docs/`. Place them under the relevant application test folder so they run in CI.

Reports:

- **Verification reports:** `docs/business/verification/` — generated artifacts showing pass/fail status for each scenario. See the Verification reporting section below.

## What goes into each

**Journey:**

- Outcome-focused narrative
- Major touchpoints in the user's experience
- Links to the scenarios derived from this journey

**Scenario:**

- Given/When/Then style expected behaviour (black box)
- Navigable links to relevant BDD test feature files — clickable for business readers, not just code-formatted paths
- Must not link to solution-layer flows (see linking direction rule in `docs.conventions.md`)

**BDD test (feature file):**

- Executable examples of a scenario
- Should reference scenario IDs (via tags or comments)
- Should validate observable behaviour, not internal orchestration

## Verification reporting

BDD test results can be surfaced as verification status within the documentation structure. This closes the loop between "what we said the system should do" and "proof that it does."

### What to generate

- **Summary (markdown)** — the primary report. Renderable on platforms like GitHub without cloning, and easily readable by agents. Should include two levels of detail:
  - A **scenario summary table** mapping scenario IDs to pass/fail counts — quick health check for PMs and stakeholders.
  - **Per-scenario sections** (one heading per scenario) listing individual test names and their results — lets business owners see exactly what is being verified. Using headings (e.g. `### SCN-xxx — Scenario Name`) also enables deep-linking from individual scenario files directly to their results.
  - A timestamp of the last run.
- **Rich report (HTML)** — optional. Generated by BDD tooling (e.g. `dotnet test` HTML logger). Provides interactive pass/fail detail, durations, and error messages. Viewable locally when the repo is cloned, or as a CI artifact.

### Where reports live

Place generated reports in `docs/business/verification/`. This is business-layer content — black-box verification of scenarios — not solution-layer. Reports sit alongside the scenarios they verify.

### Generated content markers

Mark generated sections with standard markers so agents and humans know not to hand-edit them:

```markdown
<!-- BEGIN GENERATED: description -->
(generated content here)
<!-- END GENERATED -->
```

Agents must overwrite content between these markers entirely — do not merge or append.

### Linking reports to business docs

- Individual scenario files should **deep-link** to their specific section in the verification summary (e.g. `bdd-status.md#scn-xxx--scenario-name`). This takes the reader straight to their scenario's results rather than the top of the page.
- The business context or business index can link to the verification summary for an overall health view.
- A generation script should be documented in the project's local development standards (e.g. BDD conventions).

### Report freshness

Generated reports reflect the last manual (or CI) run. Include a timestamp in the report and a note explaining that the status is point-in-time. CI automation can keep reports current, but this is not required — even manually generated reports provide value.

## Non-blocking vs blocking

This is an engagement choice. A common progression is:

- Scenarios and BDD tests may be defined early and start red.
- Passing tests become stronger gates over time as the system matures.

If tests are non-blocking initially, be explicit in CI configuration and documentation so expectations are clear.
