# Solution

## System boundary

See [solution-context.md](solution-context.md) for the system boundary, external systems, and architecture narrative.

## Applications

| Application | Short Code | Technology | Purpose | Link |
|-------------|-----------|------------|---------|------|
| Kiosk TUI | KSK | .NET console app with Terminal UI | Customer-facing self-service ordering terminal | [kiosk-tui](applications/kiosk-tui/kiosk-tui.index.md) |
| Order API | API | ASP.NET Core minimal API | Central backend — receives orders, stores state, calls payment gateway | [order-api](applications/order-api/order-api.index.md) |
| Kitchen Display | KDS | .NET console app | Staff-facing display showing active orders | [kitchen-display](applications/kitchen-display/kitchen-display.index.md) |

See [applications-context.md](applications/applications-context.md) for cross-application integration patterns.

## Scenario → flow traceability

| Scenario | Description | Flow(s) |
|----------|-------------|---------|
| [SCN-001](../business/scenarios/SCN-001-browse-menu-select-drink.md) | Browse Menu and Select Drink | [FLW-KSK-001](applications/kiosk-tui/flows/FLW-KSK-001-customer-places-order.md) |
| [SCN-002](../business/scenarios/SCN-002-customise-drink.md) | Customise Drink | [FLW-KSK-001](applications/kiosk-tui/flows/FLW-KSK-001-customer-places-order.md) |
| [SCN-003](../business/scenarios/SCN-003-payment-succeeds.md) | Payment Succeeds | [FLW-KSK-001](applications/kiosk-tui/flows/FLW-KSK-001-customer-places-order.md), [FLW-API-001](applications/order-api/flows/FLW-API-001-process-order-and-payment.md) |
| [SCN-004](../business/scenarios/SCN-004-payment-fails.md) | Payment Fails | [FLW-KSK-001](applications/kiosk-tui/flows/FLW-KSK-001-customer-places-order.md), [FLW-API-001](applications/order-api/flows/FLW-API-001-process-order-and-payment.md) |
| [SCN-005](../business/scenarios/SCN-005-kitchen-marks-order-complete.md) | Kitchen Marks Order Complete | [FLW-KDS-001](applications/kitchen-display/flows/FLW-KDS-001-display-and-complete-orders.md) |

## Decisions

See [decisions.index.md](../decisions/decisions.index.md) for the ADR catalogue.
