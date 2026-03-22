# Kitchen Display — App Context

## What is this application?

The Kitchen Display is the staff-facing order display for Compile & Sip. It shows all active orders — those that have been paid but not yet completed — with full customisation details (drink name, milk type, toppings, sugar level, ice level, temperature).

Kitchen staff work through orders in sequence and mark each one as complete when the drink is prepared. There is no digital notification to the customer — staff verbally call out the order number.

## Technology

.NET console application.

## Capabilities

| Capability | Description |
|------------|-------------|
| Display active orders | Shows paid, in-progress orders with full customisation details |
| Mark orders complete | Single action to mark an order as done, removing it from the active list |

## Dependencies

| Dependency | Direction | Description |
|------------|-----------|-------------|
| [Order API](../order-api/order-api.index.md) | Outbound | HTTP poll — fetch active orders and mark orders complete |

## Key decisions

- [ADR-0001 — .NET Platform](../../../decisions/ADR-0001-solution-dotnet-platform.md) — why .NET for all applications
- [ADR-0003 — REST/HTTP Polling](../../../decisions/ADR-0003-solution-rest-http-polling.md) — why HTTP polling instead of events
