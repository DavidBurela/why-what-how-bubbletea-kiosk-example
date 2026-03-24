# Order API (API)

**Short code:** API

Central backend service. Receives orders from the kiosk, stores them in memory, processes payment via the external gateway, serves the order list to the kitchen display, and handles order completion.

## Navigation

- [App Context](app-context.md) — technology, capabilities, and dependencies
- Flows
  - [FLW-API-001 — Process Order and Payment](flows/FLW-API-001-process-order-and-payment.md)

## Source code

`src/CompileAndSip.OrderApi/` — ASP.NET Core Minimal API with domain logic and in-memory storage
