# SCN-004 — Payment Fails

## Journeys

- [JNY-001 — Customer Places an Order](../journeys/JNY-001-customer-places-order.md)

## Given / When / Then

**Given** the customer has reviewed their order and confirmed it
**When** they present their payment card
**And** the payment is declined
**Then** they see a clear message that payment failed
**And** they are given the option to try again or cancel the order
**And** no order is sent to the kitchen

## Notes

The error message should be friendly, not technical ("Payment didn't go through — would you like to try again?"). The customer's order customisation should be preserved so they don't have to start over.

**Variant — payment provider unavailable:**

**Given** the customer has reviewed their order and confirmed it
**When** the payment provider is unavailable
**Then** they see a message that payment is temporarily unavailable
**And** they are advised to pay at the counter instead

This is an edge case but important for resilience. The kiosk should not appear broken — it should gracefully degrade. This variant may be captured as a separate BDD test rather than a separate scenario.

## Verification

BDD test: `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-004-payment-fails.feature`
