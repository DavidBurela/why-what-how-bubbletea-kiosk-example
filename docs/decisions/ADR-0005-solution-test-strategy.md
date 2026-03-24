# ADR-0005: Test Strategy — TDD + BDD at API Boundary

**Status:** Accepted

## Context

The system needs a testing approach that verifies both business intent (do we build the right thing?) and implementation correctness (does the code work?). The Order API is the central service containing all business logic.

Alternatives considered:

- **BDD against full running system** — tests all layers but slow, flaky, and requires process orchestration.
- **Domain-level BDD only** — misses integration issues at the HTTP boundary.
- **Single test project for everything** — mixes concerns, makes test isolation difficult.

## Decision

Four test projects across two verification layers:

- **BDD** (Reqnroll + xUnit) — at the API boundary via `WebApplicationFactory`. Feature files use business language only. One feature file per business scenario (SCN-001 through SCN-005).
- **TDD** (xUnit + FluentAssertions) — for domain logic, API integration, and service layers. Covers pricing, menu, orders, validation, payment, and API client behaviour.

Console/TUI rendering is not tested — only service layers and API clients.

## Consequences

- **BDD catches intent errors** — feature files read as business language, verifiable by non-developers.
- **TDD catches implementation errors** — fast, focused, deterministic.
- **Each test project mirrors or tests one source project** — clear ownership.
- **No flaky UI-dependent tests** — all tests run in-process via `WebApplicationFactory` or mock HTTP handlers.
- **Trade-off** — TUI rendering is untested. Acceptable because rendering is a thin layer over well-tested service logic.
