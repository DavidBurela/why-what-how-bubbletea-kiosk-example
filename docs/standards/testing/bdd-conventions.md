# BDD Conventions

## File naming

One `.feature` file per SCN-xxx scenario:
- `SCN-001-browse-menu-select-drink.feature`
- `SCN-002-customise-drink.feature`
- etc.

## Gherkin language

Business language only — no HTTP verbs, status codes, JSON, or API paths. Write from the customer or staff perspective.

Good: `When they order a "Classic Milk Tea" with default options`
Bad: `When they POST to /orders with {"drinkId": "classic-milk-tea"}`

## Step definitions

Step definitions are thin HTTP adapters — they translate Gherkin to API calls. No business logic in step definitions.

## Test infrastructure

- Shared `TestContext` via Reqnroll DI for per-scenario state (`HttpClient`, `FakePaymentGateway`).
- `[BeforeScenario]` hook creates `WebApplicationFactory<Program>` fresh per scenario.
- Configure payment gateway outcomes via `Given` steps (e.g., `Given the payment will be declined`).

## Test data

Use concrete data from `docs/business/menu-model.md` in scenarios — real drink names, real prices. The $8.00 full customisation example is a key verification point.

## How it works

The BDD test infrastructure boots the real ASP.NET Core application in-process and sends HTTP requests through an in-memory transport. No TCP ports are opened, no external processes are started.

```
Feature File (business language)
  → Step Definitions (thin HTTP adapters)
    → HttpClient
      → In-Memory Test Server (WebApplicationFactory)
        → Real ASP.NET Core Pipeline (routing, validation, serialization)
          → Domain Logic + In-Memory Storage
```

### WebApplicationFactory

`WebApplicationFactory<Program>` (from `Microsoft.AspNetCore.Mvc.Testing`) creates an in-memory test server running the full ASP.NET Core middleware pipeline. `CreateClient()` returns an `HttpClient` that sends real HTTP requests through the in-memory transport — routing, model binding, validation, serialization, and error handling are all exercised.

### Scenario isolation

Each scenario gets a **fresh** `WebApplicationFactory` instance via the `[BeforeScenario]` hook in `Support/Hooks.cs`. This guarantees scenario isolation — no shared state leaks between scenarios. The factory and client are disposed in the `[AfterScenario]` hook.

### Per-scenario state

`TestContext` (in `Support/TestContext.cs`) holds per-scenario state: the `HttpClient`, the last `HttpResponseMessage`, and references to test doubles like `FakePaymentGateway`. Reqnroll's built-in dependency injection provides a single `TestContext` instance to all step definition classes within a scenario.

### Test doubles

`FakePaymentGateway` is resolved from the application's DI container via `_factory.Services.GetRequiredService<FakePaymentGateway>()`. This allows `Given` steps to configure payment outcomes (success, declined, unavailable) without modifying the application code.

### Step definitions

Step definitions (in `StepDefinitions/`) receive `TestContext` via Reqnroll's constructor injection. Each step method translates a Gherkin phrase into an HTTP call — for example, "When they view the menu" becomes `await _context.Client.GetAsync("/menu")` in `StepDefinitions/MenuSteps.cs`.

## What this achieves

### Business language preserved

Feature files read as plain English scenarios. Non-developers can review and validate them. The Gherkin stays free of HTTP verbs, status codes, JSON, and API paths.

### Tests at the real service boundary

Tests exercise the same HTTP interface that real consumers (Kiosk TUI, Kitchen Display) use. If a route, DTO, or validation rule changes, BDD tests catch it immediately.

### Fast and deterministic

No network latency, no port conflicts, no process startup wait. The full BDD test suite runs in seconds because everything executes in-process via the in-memory transport.

### Contrast with alternatives

| Approach | Coverage | Speed | Reliability |
|----------|----------|-------|-------------|
| Full E2E against running processes | High — tests real deployment | Slow — process startup, ports, teardown | Flaky — port conflicts, startup races |
| Domain-level testing only | Partial — misses HTTP layer | Fast | Reliable |
| **WebApplicationFactory (this project)** | **High — full HTTP pipeline** | **Fast — in-memory** | **Reliable — no external dependencies** |

The `WebApplicationFactory` approach gives the coverage of full E2E with the speed and reliability of unit tests. The trade-off is that it does not test deployment configuration or inter-process communication — those are covered by manual E2E testing (see `testing-strategy.md`).
