# Coding Conventions

## Platform

- **.NET 10** — all projects target `net10.0`.
- **File-scoped namespaces** — one namespace declaration per file, no braces.

## Types

- **Records** for immutable value types (e.g., `MenuItem`, `OrderItem`).
- **Enums** for fixed option sets (e.g., `OrderStatus`, `DrinkSize`).
- **Sealed classes** where inheritance is not needed.

## API style

- **Minimal API** — use `app.MapGet`, `app.MapPost`, etc. No controllers.
- **Interface-based DI** for testability (e.g., `IPaymentGateway`, `IOrderStore`).

## Storage

- **`ConcurrentDictionary`** for in-memory storage (ADR-0002).
- **`Interlocked.Increment`** for atomic counters (e.g., order ID generation).

## General

- Keep code simple — this is a reference example, not production.
- Prefer clarity over cleverness.
