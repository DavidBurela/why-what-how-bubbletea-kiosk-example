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
