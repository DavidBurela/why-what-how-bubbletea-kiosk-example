# ADR-0006: Manual Orchestration over Aspire

**Status:** Accepted

## Context

The three applications need to be started for local development and manual testing. We need to decide how to orchestrate them.

Alternatives considered:

- **.NET Aspire AppHost** — one-command start with dashboard and OpenTelemetry. Adds an AppHost project, Aspire SDK dependencies, and service discovery configuration.
- **Docker Compose** — container-based orchestration. Adds Dockerfiles and compose configuration.
- **Shell orchestration with process monitoring** — more robust than a simple script but more complex.

## Decision

Applications start independently with `dotnet run`. No .NET Aspire AppHost. A convenience script (`run-all.sh`) starts all three apps. OrderApi binds to port 5100 via launchSettings; TUI apps read the URL from configuration (`OrderApi:BaseUrl`).

## Consequences

- **Zero additional projects** beyond the three applications. Project structure maps 1:1 to the architecture diagram.
- **No Aspire SDK, OpenTelemetry, or service discovery dependencies.**
- **Simple mental model** — each app is a standalone process with explicit configuration.
- **Trade-off** — no built-in dashboard or distributed tracing. Acceptable for a reference example where the documentation framework is the focus, not operational observability.
- **Trade-off** — `run-all.sh` is a fire-and-forget script with no health checks. Developers must verify the API is running before starting TUI apps.
