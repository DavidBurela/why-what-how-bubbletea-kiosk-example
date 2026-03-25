# SCN-003 — Payment Succeeds

## Journeys

- [JNY-001 — Customer Places an Order](../journeys/JNY-001-customer-places-order.md)

## Given / When / Then

**Given** the customer has reviewed their order and confirmed it
**When** they present their payment card
**Then** the payment is processed successfully
**And** they see a confirmation screen with their order number
**And** the order is sent to the kitchen display

## Notes

The order number is a simple sequential number for the day. The confirmation screen should clearly show "Your order number is #XX" and indicate they should wait for their number to be called.

## Verification

Feature file: [`SCN-003-payment-succeeds.feature`](../../../src/tests/CompileAndSip.Bdd.Tests/Features/SCN-003-payment-succeeds.feature)

Latest result: [SCN-003 verification status](../verification/bdd-status.md#scn-003--payment-succeeds)
