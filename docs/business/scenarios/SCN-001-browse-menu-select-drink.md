# SCN-001 — Browse Menu and Select Drink

## Journeys

- [JNY-001 — Customer Places an Order](../journeys/JNY-001-customer-places-order.md)

## Given / When / Then

**Given** the customer is at the kiosk home screen
**When** they navigate to the menu
**Then** they see the available drinks with names and descriptions
**And** they can select a drink to begin customising

## Notes

The menu is small (3 items) and should be immediately visible without scrolling or searching. Each drink shows its name, a brief description, and base price.

## Verification

BDD test: `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-001-browse-menu-select-drink.feature`

Current status: [BDD verification status](../verification/bdd-status.md)
