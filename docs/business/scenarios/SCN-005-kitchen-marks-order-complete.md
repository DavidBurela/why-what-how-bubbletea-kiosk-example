# SCN-005 — Kitchen Marks Order Complete

## Journeys

- [JNY-002 — Kitchen Staff Fulfills an Order](../journeys/JNY-002-kitchen-staff-fulfills-order.md)

## Given / When / Then

**Given** a paid order is displayed on the kitchen display
**When** the kitchen staff finishes preparing the drink
**And** they mark the order as complete
**Then** the order is removed from the active orders list (or moved to a completed section)
**And** the staff verbally calls out the order number

## Notes

The kitchen display shows orders in the sequence they were received. Each order shows full customisation details. Marking as complete is a single action (button press). There is no digital notification to the customer — the verbal callout is the notification mechanism. This keeps the system simple.

## Verification

BDD test: `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-005-kitchen-marks-order-complete.feature`
