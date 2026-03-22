# ADR-0002: In-Memory Storage for Order API

**Status:** Accepted

## Context

The Order API needs to store orders — at minimum, the current set of active orders that the Kitchen Display reads and the kiosk writes. The project is a reference example, and a key goal is zero infrastructure: anyone should be able to clone the repo and run the system without installing or configuring a database.

Alternatives considered:

- **SQLite** — file-based, no server required. Adds a dependency and schema management overhead for what is a small, transient dataset.
- **PostgreSQL** — production-grade, but requires a running database server (or Docker), contradicting the zero-infrastructure goal.

## Decision

Store all orders in memory within the Order API process. Orders exist only while the API is running. There is no persistence across restarts.

## Consequences

- **Zero infrastructure** — no database to install, configure, or migrate. `dotnet run` is sufficient.
- **Simplicity** — no ORM, no connection strings, no schema files. Order state is a simple in-memory collection.
- **Data loss on restart** — all orders are lost when the API process stops. Acceptable for a reference example; unacceptable for production.
- **Single instance only** — in-memory state cannot be shared across multiple API instances. Scaling out would require a shared store.
- **Migration path** — if the project evolves beyond the reference example, replacing in-memory storage with a database is a well-understood refactoring. The storage boundary can be abstracted behind a repository interface.
