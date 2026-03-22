# Kiosk TUI — App Context

## What is this application?

The Kiosk TUI is the customer-facing self-service ordering terminal for Compile & Sip. Customers interact with it to browse the drink menu, customise their order (milk, toppings, sugar, ice, temperature), review the total price, and pay via card.

It replaces the verbal order-taking process that causes queues and customisation errors during peak hours. Once payment succeeds, the kiosk displays an order number and the customer steps aside to wait.

## Technology

.NET console application with Terminal UI (Spectre.Console or similar).

## Capabilities

| Capability | Description |
|------------|-------------|
| Menu display | Shows the 3-drink menu with names, descriptions, and base prices |
| Drink customisation | Allows selection of milk type, toppings (0–2), sugar level, ice level, and temperature |
| Order review and confirmation | Displays the complete order summary with calculated total price before payment |
| Payment initiation | Submits the order to the Order API and triggers card payment processing |
| Order number display | Shows the assigned order number on a confirmation screen after successful payment |

## Dependencies

| Dependency | Direction | Description |
|------------|-----------|-------------|
| [Order API](../order-api/order-api.index.md) | Outbound | HTTP — submit orders and initiate payment |

## Key decisions

- [ADR-0001 — .NET Platform](../../../decisions/ADR-0001-solution-dotnet-platform.md) — why .NET for all applications
- [ADR-0003 — REST/HTTP Polling](../../../decisions/ADR-0003-solution-rest-http-polling.md) — why REST/HTTP communication
