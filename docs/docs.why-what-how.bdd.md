# Extension: Journeys, Scenarios, and BDD

This document explains how business behaviour is defined and verified.

## Purpose

- Journeys capture end-to-end user narrative.
- Scenarios capture testable black-box behaviour.
- BDD tests provide executable verification of scenarios.

This is used to track progress and reduce ambiguity about expected outcomes.

## Behaviour stack

- Journey -> 1 to many Scenarios -> 1 to many BDD tests

Key points:

- Journeys are narrative and cross-application.
- Scenarios are behavioural slices that must be observable externally.
- BDD tests are concrete executable examples that prove scenarios hold.

Mappings are not necessarily 1:1:

- A journey usually requires multiple scenarios.
- A scenario often requires multiple BDD tests (variants, edge cases, roles).
- A single flow may support multiple scenarios.

## Where things live

Docs:

- Journeys: `docs/business/journeys/`
- Scenarios: `docs/business/scenarios/`

Tests:

- BDD `.feature` files live with code (not under `docs/`).
- Place them under the relevant application test folder so they run in CI.

## What goes into each

Journey:

- Outcome-focused narrative
- Major touchpoints
- Links to scenarios

Scenario:

- Given/When/Then style expected behaviour (black box)
- Links to relevant BDD test paths (feature files)
- Must not link to solution-layer flows (see linking direction rule in `docs.conventions.md`)

BDD tests:

- Executable examples of a scenario
- Should reference scenario IDs (via tags or comments)
- Should validate observable behaviour, not internal orchestration

## Non-blocking vs blocking

This is an engagement choice. A common progression is:

- Scenarios and BDD tests may be defined early and start red.
- Passing tests become stronger gates over time as the system matures.

If tests are non-blocking initially, be explicit in CI configuration and documentation so expectations are clear.
