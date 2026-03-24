# ADR-0007: Simulated Payment Gateway via Interface

**Status:** Accepted

## Context

The Order API needs to process payments as part of the order flow. The system boundary includes a payment gateway as an external dependency, but the reference example should not require real payment credentials or external services.

Alternatives considered:

- **Separate HTTP stub service** — a standalone mock server. Adds another process to manage and another project to the solution.
- **Test doubles only in test project** — the fake only exists during testing, so the main app has no payment implementation at all.

## Decision

`IPaymentGateway` interface and `FakePaymentGateway` both live in the OrderApi project. The fake is configurable for success, decline, and unavailable outcomes. BDD and integration tests configure it via DI to verify all payment paths.

## Consequences

- **Zero infrastructure** for payment simulation. No external processes or services.
- **BDD can test all payment paths** — success, decline, and unavailable — by configuring the fake gateway per scenario.
- **Clean interface boundary** — `IPaymentGateway` can be replaced with a real implementation when a payment provider is integrated.
- **The interface is internal to OrderApi** — TUI apps see only HTTP responses (201, 402, 503). They have no knowledge of the payment gateway.
- **Trade-off** — the fake lives in production code, not just test code. Acceptable because this is a reference example, and the fake is the intended implementation.
