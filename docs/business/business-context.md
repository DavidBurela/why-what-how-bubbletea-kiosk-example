# Business Context

## Problem statement

Compile & Sip faces two interconnected problems that compound each other during peak hours.

**Long queues during peak hours.** During busy periods, the queue stretches out the door. An estimated 20–30% of potential peak-hour customers walk away rather than wait. The bottleneck is the ordering process itself — a staff member taking orders verbally at the counter can only work as fast as a human can take a complex order, confirm it, and process payment.

**Order customisation errors.** Bubble tea orders are highly customisable — milk type, toppings, sugar level, ice level, temperature. When staff take orders verbally during a rush, mistakes happen: the wrong milk, forgotten toppings, incorrect sugar level. Each error means a wasted drink, a frustrated customer, and time spent remaking the order.

These problems feed each other. Queues create pressure, pressure causes errors, errors slow things down, and queues get longer. The owner has tried adding more staff, but the bottleneck is the ordering process itself, not the preparation speed.

## Stakeholders

| Role | Interest | What they care about |
|------|----------|----------------------|
| **Shop owner (Mei)** | Revenue, efficiency, customer satisfaction, staff wellbeing | Reducing lost customers from queue abandonment. Reducing drink waste from errors. Letting staff focus on preparation quality. Keeping the friendly shop atmosphere despite automation. |
| **Customers** | Convenience, accuracy, speed, good experience | Getting the drink they actually want. Not waiting in long queues. Being able to customise without feeling rushed. Clear communication about when their order is ready. |

## Background

Compile & Sip is a small, independent bubble tea shop in a busy urban neighbourhood. Founded by a tea enthusiast who started with a market stall and grew into a permanent shopfront, the shop has developed a loyal following for its quality drinks and customisation options. The brand is friendly, a bit nerdy, and approachable — "your neighbourhood bubble tea spot" rather than a sleek chain.

The shop has been open for two years. Business is good — almost too good. Peak hours (lunch, after school, weekends) create problems that the owner can no longer solve by just adding staff.

## What it IS / What it IS NOT

| What it IS | What it IS NOT |
|------------|----------------|
| A self-service touchscreen kiosk that augments counter service | NOT a staff replacement — staff remain available for customers who prefer human interaction |
| A way for customers to browse, customise, and pay at their own pace | NOT a mobile ordering system |
| A kitchen display providing clear, unambiguous order details to staff | NOT a full point-of-sale system |
| A single-shop ordering solution | NOT a chain management or multi-location system |

## How Might We questions

1. **How might we reduce queue wait times during peak hours?**
   A self-service kiosk removes the staff ordering bottleneck. Customers order in parallel.

2. **How might we eliminate order customisation errors?**
   The customer selects options directly — no verbal relay, no interpretation. What they pick is what the kitchen sees.

3. **How might we let customers order at their own pace without a staff bottleneck?**
   The kiosk doesn't have a queue behind it pressuring the customer. They browse, customise, and confirm on their own terms.

## Verification

Business scenarios are verified via executable BDD tests that run against the API boundary. Each scenario defined in [Scenarios](scenarios/scenarios.index.md) has a corresponding feature file containing Given/When/Then examples written in business language.

- [Verification status](verification/bdd-status.md) — pass/fail results for all business scenarios
- [How BDD verification works](../docs.why-what-how.bdd.md) — methodology and structure
