# SCN-002 — Customise Drink

## Journeys

- [JNY-001 — Customer Places an Order](../journeys/JNY-001-customer-places-order.md)

## Given / When / Then

**Given** the customer has selected a drink
**When** they are on the customisation screen
**Then** they can choose their milk type (Regular, Oat, None)
**And** they can select 0 to 2 toppings from the available options (Tapioca Pearls, Coconut Jelly, Pudding)
**And** they can set their sugar level (0%, 25%, 50%, 75%, 100%)
**And** they can set their ice level (No Ice, Less Ice, Regular Ice)
**And** they can choose temperature (Hot or Cold)
**And** they see the total price updated as they customise
**And** they can proceed to review their order

## Notes

Default selections should be sensible (e.g., Regular milk, 100% sugar, Regular ice, Cold). Toppings are optional. Price adjustments for milk alternatives or toppings should be reflected in real time.

## Verification

Feature file: [`SCN-002-customise-drink.feature`](../../../src/tests/CompileAndSip.Bdd.Tests/Features/SCN-002-customise-drink.feature)

Latest result: [SCN-002 verification status](../verification/bdd-status.md#scn-002--customise-drink)
