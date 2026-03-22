# Order API — App Context

## What is this application?

The Order API is the central backend for the Compile & Sip Kiosk System. It receives orders from the Kiosk TUI, stores them in memory, calls the payment gateway to process card payments, and serves the list of active orders to the Kitchen Display.

It is the single source of truth for order state. All other applications interact with it via HTTP — the kiosk submits orders, and the kitchen display reads and updates them.

## Technology

.NET web API (ASP.NET Core minimal API).

## Capabilities

| Capability | Description |
|------------|-------------|
| Receive orders | Accepts completed orders (drink + customisations) from the Kiosk TUI |
| In-memory storage | Stores all orders in memory for the lifetime of the process |
| Payment processing | Calls the external payment gateway to process card payments |
| Serve order list | Provides active orders (paid, not yet completed) to the Kitchen Display |
| Mark orders complete | Updates order status when kitchen staff mark an order as done |

## Dependencies

| Dependency | Direction | Description |
|------------|-----------|-------------|
| Payment Gateway | Outbound | HTTP — process card payment (simulated external service) |
| [Kiosk TUI](../kiosk-tui/kiosk-tui.index.md) | Inbound | HTTP — receives orders and payment requests |
| [Kitchen Display](../kitchen-display/kitchen-display.index.md) | Inbound | HTTP — serves active orders, accepts completion updates |

## Key decisions

- [ADR-0001 — .NET Platform](../../../decisions/ADR-0001-solution-dotnet-platform.md) — why .NET for all applications
- [ADR-0002 — In-Memory Storage](../../../decisions/ADR-0002-application-order-api-in-memory-storage.md) — why no database
- [ADR-0003 — REST/HTTP Polling](../../../decisions/ADR-0003-solution-rest-http-polling.md) — why REST/HTTP communication
