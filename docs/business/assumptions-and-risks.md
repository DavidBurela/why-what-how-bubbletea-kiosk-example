# Assumptions and Risks

## Assumptions

| # | Assumption | Status |
|---|-----------|--------|
| A1 | Customers are comfortable using a touchscreen self-service kiosk | Assumed |
| A2 | The shop has reliable internet/network for payment processing | Assumed |
| A3 | Card payment is the only payment method needed (no cash at kiosk) | Assumed |
| A4 | A small menu (3 drinks) with customisation provides enough variety | Assumed |
| A5 | Verbal callout is sufficient for order-ready notification (no digital notification needed) | Assumed |
| A6 | A single kiosk is sufficient for the shop's peak traffic | Assumed |

## Risks

| # | Risk | Impact | Mitigation |
|---|------|--------|-----------|
| R1 | Customers may find the kiosk impersonal and prefer human interaction | Reduced adoption, staff still busy taking orders | Staff remain available for those who prefer counter service; kiosk is an addition, not a replacement |
| R2 | Payment provider downtime blocks all kiosk orders | No kiosk orders possible during outage | Graceful error message directing customers to pay at the counter |
| R3 | Kitchen display becomes a single point of failure for order communication | Orders placed but not visible to kitchen | If display fails, staff revert to verbal orders from the counter |
