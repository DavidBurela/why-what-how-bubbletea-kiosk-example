# JNY-001 — Customer Places an Order

## Persona

[Customer — Alex](../personas/customer.md)

## Goal

Order a customised bubble tea drink and pay, without waiting in a queue or worrying about order accuracy.

## Trigger

Customer walks into Compile & Sip and approaches the self-service kiosk.

## Steps

1. Customer walks up to the kiosk and sees the welcome/home screen.
2. Customer browses the drink menu (3 drinks displayed with descriptions).
3. Customer selects a drink (e.g., Taro Milk Tea).
4. Customer customises the drink:
   - Chooses milk type (Regular, Oat, or None)
   - Selects toppings (0–2 from: Tapioca Pearls, Coconut Jelly, Pudding)
   - Sets sugar level (0%, 25%, 50%, 75%, 100%)
   - Sets ice level (No Ice, Less Ice, Regular Ice)
   - Chooses temperature (Hot or Cold)
5. Customer reviews the order summary (drink name, all customisations, price).
6. Customer confirms and proceeds to payment.
7. Customer pays via card (contactless or chip).
8. Payment is processed successfully.
9. Customer receives an order number on screen.
10. Customer steps aside and waits.
11. Kitchen staff calls out the order number verbally when the drink is ready.
12. Customer picks up their drink.

## Success criteria

Customer receives the exact drink they configured, with minimal wait time for the ordering process itself (under 2 minutes from approach to payment complete).

## Linked scenarios

- [SCN-001 — Browse Menu and Select Drink](../scenarios/SCN-001-browse-menu-select-drink.md)
- [SCN-002 — Customise Drink](../scenarios/SCN-002-customise-drink.md)
- [SCN-003 — Payment Succeeds](../scenarios/SCN-003-payment-succeeds.md)
- [SCN-004 — Payment Fails](../scenarios/SCN-004-payment-fails.md)
