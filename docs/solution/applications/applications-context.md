# Applications Context

## Integration pattern

All applications communicate via REST/HTTP. There is no message broker, no WebSocket infrastructure, and no shared database. Each application is a standalone .NET process.

```
[Kiosk TUI] ──HTTP──▶ [Order API] ──HTTP──▶ [Payment Gateway (external)]
                            ▲
                            │ HTTP (poll)
                            │
                    [Kitchen Display]
```

## Relationship details

| From | To | Direction | Protocol | What flows |
|------|----|-----------|----------|------------|
| Kiosk TUI | Order API | Outbound | HTTP | Submit orders, initiate payment |
| Order API | Payment Gateway | Outbound | HTTP | Process card payment (simulated) |
| Kitchen Display | Order API | Outbound | HTTP (poll) | Fetch active orders, mark orders complete |

- **Kiosk TUI → Order API:** The kiosk submits a completed order (drink selection + all customisations) and triggers payment processing. The Order API returns the result (success with order number, or payment failure).
- **Order API → Payment Gateway:** The Order API calls the external payment gateway to process the customer's card payment. This is simulated — the gateway is not a real service.
- **Kitchen Display → Order API:** The kitchen display polls the Order API at intervals to retrieve the list of active (paid, not yet completed) orders. When staff finish preparing a drink, the display sends a request to mark that order as complete.

## Future integration

If the project evolves beyond the reference example toward the full vision:

- **Event-driven communication** could replace HTTP polling for the Kitchen Display, providing real-time order updates without polling overhead.
- **Mobile ordering app** would be an additional client of the Order API, submitting orders alongside the kiosk.
- **Loyalty service** would be a new system integrated with the Order API to track customer rewards and preferences.
