# Phase 4: Depth (conditional)

Add flows, C4 model, and fill the traceability table. Only proceed if Phases 1-3 are confirmed solid.

## Prerequisites

Phases 1, 2, and 3 must be complete. The following should already exist:
- `docs/business/scenarios/` — SCN-001 through SCN-005
- `docs/solution/solution.index.md` — with scenario→flow traceability table (flows showing "—")
- `docs/solution/solution-context.md` — system boundary and architecture narrative
- `docs/solution/applications/` — 3 app folders with landing pages and app-context files
- `docs/decisions/` — ADR-0001 through ADR-0003

Verify these exist before starting.

## Context — read these files first

Read these files in order before creating anything. They provide the source material, templates, and structural rules.

| # | File | What to extract |
|---|------|-----------------|
| 1 | `agent-temp/backstory-compile-and-sip.md` | **§6** (Journey steps — the runtime sequences to decompose into flows), **§7** (Scenarios — what each flow must support), **§9** (Solution Shape — communication pattern, application responsibilities) |
| 2 | `docs/docs.why-what-how.area-guidance.md` | **Flows section** — expected sections for flow files (Summary, Participants, Sequence, Scenarios link, C4 dynamic view reference). |
| 3 | `docs/docs.why-what-how.c4model.md` | **Structurizr DSL conventions** — workspace structure, element naming, relationship syntax, dynamic view syntax. Use as your template for `workspace.dsl`. |
| 4 | `docs/docs.conventions.md` | **Flow naming** — `FLW-<APP>-<id>-<name>.md`. **Level field** — `context`, `container`, or `component`. **Generated artifacts** — export path conventions. |
| 5 | `docs/docs.structure.md` | **Target tree** — flow paths under `docs/solution/applications/<app>/flows/`. C4 model path: `docs/solution/c4-model/`. **Diagram expectations** per level. |
| 6 | `docs/docs.bootstrap.md` | **Phase 4 section** — what to add and when each type earns its place. |
| 7 | `work/001-documentation-bootstrap/README.md` | **Decisions D6, D7** — Phase 4 is conditional; glossary only if it earns its place. |

Also read key solution files to understand the existing structure:
- `docs/solution/solution.index.md` — current traceability table to fill in
- `docs/solution/applications/applications-context.md` — integration pattern (flows must align)
- `docs/solution/applications/kiosk-tui/app-context.md` — KSK capabilities
- `docs/solution/applications/order-api/app-context.md` — API capabilities
- `docs/solution/applications/kitchen-display/app-context.md` — KDS capabilities

And the scenario files to ensure flow-to-scenario links are correct:
- `docs/business/scenarios/scenarios.index.md`

## What to create

| # | Action | File |
|---|--------|------|
| 4.1 | Create | `docs/solution/applications/kiosk-tui/flows/FLW-KSK-001-customer-places-order.md` |
| 4.2 | Create | `docs/solution/applications/order-api/flows/FLW-API-001-process-order-and-payment.md` |
| 4.3 | Create | `docs/solution/applications/kitchen-display/flows/FLW-KDS-001-display-and-complete-orders.md` |
| 4.4 | Create | `docs/solution/c4-model/workspace.dsl` |
| 4.5 | Update | `docs/solution/solution.index.md` — fill scenario→flow traceability table |
| 4.6 | Consider | `docs/business/glossary.md` — create only if terms are referenced ambiguously across documents |
| 4.7 | Consider | Standards — scan ADRs for patterns; likely defer |

## Content mapping

Use the **expected sections** defined in `docs.why-what-how.area-guidance.md` § Flows as your template for each flow file. Below is guidance on what backstory content maps into each flow.

**`FLW-KSK-001-customer-places-order.md`** — component-level flow within Kiosk TUI
- **Level:** component
- **Summary** — covers the kiosk-side ordering flow: display menu → customer selects drink → customise → review → confirm → submit to Order API → receive order number → display confirmation.
- **Participants** — Customer (actor), Kiosk TUI (menu screen, customisation screen, review screen, confirmation screen), Order API (external dependency).
- **Sequence** — decompose backstory §6.1 steps 1-9 into the runtime interactions within the kiosk and between the kiosk and Order API.
- **Scenarios** — links to SCN-001, SCN-002, SCN-003, SCN-004.
- **C4 dynamic view** — reference the view key in workspace.dsl.

**`FLW-API-001-process-order-and-payment.md`** — container-level flow across Order API and Payment Gateway
- **Level:** container
- **Summary** — covers the order processing and payment flow: receive order from kiosk → validate → call payment gateway → handle success/failure → store order → return result.
- **Participants** — Kiosk TUI (caller), Order API (orchestrator), Payment Gateway (external system).
- **Sequence** — decompose the Order API's role from backstory §9: receive order, call payment gateway, store in memory, return order number.
- **Scenarios** — links to SCN-003, SCN-004.
- **C4 dynamic view** — reference the view key in workspace.dsl.

**`FLW-KDS-001-display-and-complete-orders.md`** — component-level flow within Kitchen Display
- **Level:** component
- **Summary** — covers the kitchen display flow: poll Order API → display active orders → staff marks complete → update via Order API → verbal callout.
- **Participants** — Kitchen Staff (actor), Kitchen Display (display, polling loop), Order API (data source).
- **Sequence** — decompose backstory §6.2 steps 1-5 into runtime interactions.
- **Scenarios** — links to SCN-005.
- **C4 dynamic view** — reference the view key in workspace.dsl.

**`workspace.dsl`** — Structurizr DSL. Use `docs.why-what-how.c4model.md` as your template.
- **Persons:** Customer (Alex — orders drinks via kiosk), Kitchen Staff (Jamie — prepares drinks using kitchen display)
- **Software Systems:** Compile & Sip Kiosk System (internal), Payment Gateway (external, simulated)
- **Containers (within Kiosk System):** Kiosk TUI (.NET console/TUI), Order API (ASP.NET Core minimal API), Kitchen Display (.NET console)
- **Relationships:** Customer → Kiosk TUI (places orders), Kiosk TUI → Order API (submits orders via HTTP), Order API → Payment Gateway (processes payments via HTTP), Kitchen Display → Order API (polls for orders via HTTP), Kitchen Staff → Kitchen Display (views and completes orders)
- **Dynamic views:** One per flow (FLW-KSK-001, FLW-API-001, FLW-KDS-001)

**Traceability table update** — fill in the "—" entries in `solution.index.md`:

| Scenario | Flow(s) |
|----------|---------|
| SCN-001 | FLW-KSK-001 |
| SCN-002 | FLW-KSK-001 |
| SCN-003 | FLW-KSK-001, FLW-API-001 |
| SCN-004 | FLW-KSK-001, FLW-API-001 |
| SCN-005 | FLW-KDS-001 |

**Glossary** — only create `docs/business/glossary.md` if, after reviewing all business documents, terms like "order number", "topping", "sugar level" are used ambiguously or inconsistently. The 7 terms from backstory §15 are candidates. If all docs are clear without a glossary, defer.

**Standards** — scan the 3 accepted ADRs for recurring patterns. If clear standards emerge (e.g., "all services use .NET", "all communication is REST/HTTP"), consider creating standards files. Likely too early — defer unless patterns are obvious.

## Critical rules

- **Flows are white-box.** They describe internal orchestration — how things happen inside the system. This is distinct from scenarios which describe what happens from the outside.
- **Flow links back to scenarios.** Each flow doc links to the business scenario(s) it supports. This is the correct cross-layer direction (solution → business). Per `docs.conventions.md`.
- **Follow flow naming exactly.** `FLW-KSK-001-customer-places-order.md` — short code from the owning application, sequential ID. Per `docs.conventions.md`.
- **C4 model is the source of truth for diagrams.** The workspace.dsl defines structure; diagrams are exported from it. Per `docs.why-what-how.c4model.md`.

## Done when

Each of the 3 applications has at least one flow. The C4 `workspace.dsl` defines the system context, container, and dynamic views. The traceability table in `solution.index.md` maps all 5 scenarios to their supporting flows.

**Verify:**
1. Each flow file has Summary, Participants, Sequence, Scenarios, and C4 dynamic view reference sections.
2. The traceability table has no "—" entries remaining.
3. `workspace.dsl` is syntactically valid Structurizr DSL.
4. Flow-to-scenario links use correct relative paths to business scenario files.
5. No business files were modified.
