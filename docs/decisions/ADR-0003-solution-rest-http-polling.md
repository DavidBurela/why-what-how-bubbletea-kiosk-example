# ADR-0003: REST/HTTP with Polling

**Status:** Accepted

## Context

The three applications in the Compile & Sip Kiosk System need to communicate. The Kiosk TUI submits orders to the Order API, and the Kitchen Display needs to know about new and updated orders. The project is a reference example with a zero-infrastructure goal.

The Kitchen Display's need for near-real-time order updates is the key design question: how does it learn about new orders?

Alternatives considered:

- **WebSockets / SignalR** — provides real-time push from the Order API to the Kitchen Display. Adds connection management complexity and a dependency on SignalR infrastructure.
- **Message queue (RabbitMQ, etc.)** — decouples producers and consumers with durable messaging. Requires a running broker, contradicting the zero-infrastructure goal.

## Decision

All applications communicate via REST/HTTP. The Kitchen Display polls the Order API at regular intervals to fetch the current list of active orders.

## Consequences

- **No additional infrastructure** — no message broker, no WebSocket server. All communication is standard HTTP request/response.
- **Simple to understand** — every interaction is a familiar HTTP call. Easy to inspect, debug, and test.
- **Polling overhead** — the Kitchen Display makes periodic requests even when no orders have changed. Acceptable at the scale of a single kiosk; wasteful at higher volumes.
- **Latency** — new orders appear on the Kitchen Display after the next poll interval, not instantly. For a small shop with a single kiosk, a short poll interval (e.g., 2–3 seconds) makes this negligible.
- **Migration path** — replacing polling with SignalR or an event-driven approach is a targeted change to the Kitchen Display and Order API, without affecting the Kiosk TUI or the overall system boundary.
