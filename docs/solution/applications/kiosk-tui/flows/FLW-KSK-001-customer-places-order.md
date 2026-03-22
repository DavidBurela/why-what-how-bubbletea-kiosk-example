# FLW-KSK-001: Customer Places Order

**Level:** component

## Summary

Covers the complete kiosk-side ordering flow from the customer approaching the kiosk through to receiving an order number. The Kiosk TUI guides the customer through menu browsing, drink customisation, order review, and payment — delegating order submission and payment processing to the Order API.

## Participants

| Participant | Role |
|-------------|------|
| Customer | Actor — interacts with the kiosk to place an order |
| Kiosk TUI — Home Screen | Displays the welcome screen and entry point |
| Kiosk TUI — Menu Screen | Displays the drink catalogue with names, descriptions, and prices |
| Kiosk TUI — Customisation Screen | Presents customisation options (milk, toppings, sugar, ice, temperature) with live price updates |
| Kiosk TUI — Review Screen | Shows the complete order summary and total price for confirmation |
| Kiosk TUI — Confirmation Screen | Displays the assigned order number after successful payment |
| Order API | External dependency — receives the submitted order and processes payment |

## Sequence

1. Customer approaches the kiosk and sees the **Home Screen**.
2. Customer navigates to the **Menu Screen**, which displays the 3-drink menu with names, descriptions, and base prices.
3. Customer selects a drink (e.g., Taro Milk Tea).
4. The **Customisation Screen** appears with default selections (Regular milk, 100% sugar, Regular ice, Cold, no toppings).
5. Customer adjusts customisation options — milk type, toppings (0–2), sugar level, ice level, temperature. The total price updates in real time as selections change.
6. Customer proceeds to the **Review Screen**, which shows the drink name, all customisations, and the calculated total price.
7. Customer confirms the order.
8. Kiosk TUI submits the order to the **Order API** via HTTP (drink selection + all customisations).
9. Order API processes payment via the payment gateway and returns the result.
10. **On success:** The **Confirmation Screen** displays the assigned order number (e.g., "#12") and instructs the customer to wait for their number to be called.
11. **On payment failure:** The kiosk displays a friendly error message ("Payment didn't go through — would you like to try again?"). The customer's order is preserved — they can retry payment or cancel without re-customising.
12. **On gateway unavailable:** The kiosk displays a message that payment is temporarily unavailable and advises the customer to pay at the counter.

## Scenarios

This flow supports the following business scenarios:

- [SCN-001 — Browse Menu and Select Drink](../../../../business/scenarios/SCN-001-browse-menu-select-drink.md) (steps 1–3)
- [SCN-002 — Customise Drink](../../../../business/scenarios/SCN-002-customise-drink.md) (steps 4–6)
- [SCN-003 — Payment Succeeds](../../../../business/scenarios/SCN-003-payment-succeeds.md) (steps 7–10)
- [SCN-004 — Payment Fails](../../../../business/scenarios/SCN-004-payment-fails.md) (steps 7–8, 11–12)

## C4 dynamic view

View key: `FLW-KSK-001-customer-places-order` in [workspace.dsl](../../../c4-model/workspace.dsl).
