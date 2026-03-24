# Testing Strategy

## Three layers

| Layer | Tool | Scope | Catches |
|-------|------|-------|---------|
| BDD | Reqnroll + xUnit | API boundary (HTTP) | Intent errors — does the system do what business expects? |
| TDD | xUnit + FluentAssertions | Domain logic, service layers | Implementation errors — does the code do what the developer expects? |
| Manual E2E | Human walks through TUI | Full system (all 3 apps running) | Integration errors — do the apps work together? |

## Key decisions

- BDD tests run against the API via `WebApplicationFactory` — no external process needed.
- Console/TUI rendering is NOT tested — only service layers and API clients.
- All tests must be deterministic — no random data, no time-dependent assertions.
- Domain types live in OrderApi; TUI apps communicate via HTTP with their own DTOs.

## Test projects

| Project | Tests |
|---------|-------|
| `CompileAndSip.OrderApi.Tests` | Domain TDD (pricing, menu, order, validation) + API integration |
| `CompileAndSip.Bdd.Tests` | BDD features mapping to SCN-001 through SCN-005 |
| `CompileAndSip.KioskTui.Tests` | Kiosk service layer (API client, order flow logic) |
| `CompileAndSip.KitchenDisplay.Tests` | Kitchen display service layer (polling, order tracking) |
