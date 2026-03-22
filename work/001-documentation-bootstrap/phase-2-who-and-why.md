# Phase 2: Who and Why

Define personas, journeys, scenarios, and the menu domain. All content is black-box.

## Prerequisites

Phase 1 must be complete. The following files should already exist under `docs/business/`: `business.index.md`, `business-context.md`, `vision.md`, `outcomes.md`, `scope.md`, `assumptions-and-risks.md`. Verify before starting.

## Context — read these files first

Read these files in order before creating anything. They provide the source material, templates, and structural rules.

| # | File | What to extract |
|---|------|-----------------|
| 1 | `agent-temp/backstory-compile-and-sip.md` | **§5** (Personas — Alex and Jamie), **§6** (Journeys — JNY-001 and JNY-002), **§7** (Scenarios — SCN-001 through SCN-005), **§8** (Menu and Customisation Details), **§16** (Agent instructions — Business rules) |
| 2 | `docs/docs.why-what-how.area-guidance.md` | **Personas section** — expected sections for persona files. **Journeys section** — expected sections for journey files. **Scenarios section** — expected sections for scenario files. Use these as your templates. |
| 3 | `docs/docs.conventions.md` | **Journey naming** — `JNY-<id>-<name>.md`. **Scenario naming** — `SCN-<id>-<name>.md`. **Index file conventions** — catalogues need metadata tables. **Linking direction rule** — within same layer, link freely; business never points into solution. |
| 4 | `docs/docs.structure.md` | **Target tree** — confirms folder paths (`docs/business/personas/`, `journeys/`, `scenarios/`). |
| 5 | `docs/docs.bootstrap.md` | **Phase 2 section** — creation guidance, completion criteria, what to defer. |
| 6 | `docs/docs.why-what-how.bdd.md` | **Scenario format** — black-box Given/When/Then conventions. |
| 7 | `work/001-documentation-bootstrap/README.md` | **Decisions D1, D2** — menu as domain doc (`menu.md`); SCN-004 variant stays in Notes section. |

Also skim the Phase 1 output files to understand what's already documented:
- `docs/business/business-context.md` — problem, stakeholders, HMW (for consistency)
- `docs/business/scope.md` — in/out scope (scenarios should align)

## What to create

| # | Action | File | Source from backstory |
|---|--------|------|----------------------|
| 2.1 | Create | `docs/business/personas/personas.index.md` | Catalogue: Alex (primary), Jamie (secondary) |
| 2.2 | Create | `docs/business/personas/alex.md` | §5.1 |
| 2.3 | Create | `docs/business/personas/jamie.md` | §5.2 |
| 2.4 | Create | `docs/business/journeys/journeys.index.md` | Catalogue table |
| 2.5 | Create | `docs/business/journeys/JNY-001-customer-places-order.md` | §6.1 (12 steps, linked SCN-001..004) |
| 2.6 | Create | `docs/business/journeys/JNY-002-kitchen-staff-fulfills-order.md` | §6.2 (5 steps, linked SCN-005) |
| 2.7 | Create | `docs/business/scenarios/scenarios.index.md` | Catalogue mapping each SCN to journey(s) |
| 2.8 | Create | `docs/business/scenarios/SCN-001-browse-menu-select-drink.md` | §7 |
| 2.9 | Create | `docs/business/scenarios/SCN-002-customise-drink.md` | §7 |
| 2.10 | Create | `docs/business/scenarios/SCN-003-payment-succeeds.md` | §7 |
| 2.11 | Create | `docs/business/scenarios/SCN-004-payment-fails.md` | §7 (includes gateway-unavailable variant in Notes) |
| 2.12 | Create | `docs/business/scenarios/SCN-005-kitchen-marks-order-complete.md` | §7 |
| 2.13 | Create | `docs/business/menu.md` | §8 (drink catalogue, customisation options, pricing rules) |
| 2.14 | Update | `docs/business/business.index.md` | Add links to personas/, journeys/, scenarios/, menu.md |
| 2.15 | Update | `docs/docs.index.md` | Deepen Business section links |

## Content mapping

Use the **expected sections** defined in `docs.why-what-how.area-guidance.md` as your template for each file type. Below is guidance on what backstory content maps into each section.

**Persona files** (`alex.md`, `jamie.md`) — template: area-guidance § Personas
- **Who they are** — role and background from backstory §5.
- **What they care about** — motivations and priorities.
- **What they want from this system** — desired outcomes.
- **Pain points** — current frustrations (backstory provides these for both personas).
- **Linked journeys** — Alex → JNY-001, Jamie → JNY-002. Use relative links within the business layer.

**Journey files** — template: area-guidance § Journeys
- **Persona** — link to the persona file.
- **Goal** — one sentence from backstory §6.
- **Trigger** — what starts the journey.
- **Steps** — numbered narrative sequence. JNY-001 has 12 steps, JNY-002 has 5 steps.
- **Success criteria** — from backstory §6 (e.g., <2 min for JNY-001).
- **Linked scenarios** — JNY-001 links SCN-001..004, JNY-002 links SCN-005.

**Scenario files** — template: area-guidance § Scenarios
- **Journeys** — links to the journey(s) this scenario supports.
- **Given / When / Then** — black-box expected behaviour from backstory §7.
- **Notes** — clarifications/edge cases from backstory §7.
- SCN-004: include the "payment gateway unavailable" variant in the Notes section (not as a separate file — per Decision D2).

**`menu.md`** — domain document (project-specific name per area-guidance § domain-model.md)
- **Drinks** — catalogue table: name, description, base price (3 drinks from backstory §8).
- **Customisation options** — table: category, options, defaults, price impact (backstory §8).
- **Pricing rules** — how the total is calculated (base + modifiers). Include the example calculation from §8.
- This is black-box domain knowledge — describes what the shop sells and how customisation works, not how the system stores or renders it.

**Index files** — per `docs.conventions.md` index file conventions:
- `personas.index.md` — catalogue table with name, tier (primary/secondary), and link.
- `journeys.index.md` — catalogue table with ID, name, persona, and link.
- `scenarios.index.md` — catalogue table with ID, name, journey(s), and link.

## Critical rules

- **All content is black-box.** No HTTP, API endpoints, system internals, or technology words. Scenarios describe observable behaviour only. See backstory §16 Business rules.
- **Business never links into Solution.** Scenarios must NOT link to flows. Per `docs.conventions.md` linking direction rule.
- **Within-layer links are bidirectional.** Journeys link to scenarios, scenarios link back to journeys. Personas link to journeys, journeys link to personas. Per `docs.conventions.md`.
- **Follow naming conventions exactly.** `JNY-001-customer-places-order.md`, `SCN-001-browse-menu-select-drink.md`, etc. Per `docs.conventions.md`.

## Done when

Alex (primary persona) has JNY-001 with 4 derived scenarios (SCN-001..004). Jamie (secondary persona) has JNY-002 with 1 derived scenario (SCN-005). Menu domain is documented. Every scenario reads clearly to someone who knows nothing about .NET.

**Verify:**
1. Grep all files under `docs/business/` for `.NET|TUI|console app|REST|HTTP|ASP\.NET` — should return zero matches.
2. No file under `docs/business/` contains a relative link pointing into `docs/solution/`.
3. JNY-001 and JNY-002 are cross-referenced from their persona files.
4. SCN-001..005 are cross-referenced from their journey files.
