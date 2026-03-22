# Phase 1: Business Foundation

Create the core business identity and boundaries.

## Context — read these files first

Read these files in order before creating anything. They provide the source material and the structural rules.

| # | File | What to extract |
|---|------|-----------------|
| 1 | `agent-temp/backstory-compile-and-sip.md` | **§1** (Business Identity), **§2** (The Problem), **§3** (The Vision), **§4** (Stakeholders), **§10** (MVP Definition), **§11** (Scope), **§12** (HMW questions), **§13** (Assumptions and Risks), **§16** (Agent instructions — especially the Business rules) |
| 2 | `docs/docs.why-what-how.area-guidance.md` | **Business section** — expected sections for `business-context.md`, `vision.md`, `outcomes.md`, `scope.md`, `assumptions-and-risks.md`. Use these as your templates. |
| 3 | `docs/docs.conventions.md` | **Index file conventions** — what goes in `business.index.md`. **Linking direction rule** — business never points into solution. |
| 4 | `docs/docs.structure.md` | **Target tree** — confirms folder paths (`docs/business/`). **Area responsibilities** — business = intent and black-box expectations. |
| 5 | `docs/docs.bootstrap.md` | **Phase 1 section** — creation checklist, completion criteria, and what to defer. |
| 6 | `work/001-documentation-bootstrap/README.md` | **Decisions D1, D3** — menu domain doc (Phase 2), assumptions-and-risks included in Phase 1. |

## What to create

| # | Action | File | Source from backstory |
|---|--------|------|----------------------|
| 1.1 | Create | `docs/business/business.index.md` | Navigation router linking to all business files |
| 1.2 | Create | `docs/business/business-context.md` | §1 (Identity), §2 (Problem), §4 (Stakeholders), §12 (HMW) |
| 1.3 | Create | `docs/business/vision.md` | §3 (north star + longer-term direction) |
| 1.4 | Create | `docs/business/outcomes.md` | §10 (MVP definition, success criteria, exclusions) |
| 1.5 | Create | `docs/business/scope.md` | §11 (in/out scope tables) |
| 1.6 | Create | `docs/business/assumptions-and-risks.md` | §13 (6 assumptions, 3 risks) |
| 1.7 | Update | `docs/docs.index.md` | Replace "XYZ project" → "Compile & Sip", add section links |

## Content mapping

Use the **expected sections** defined in `docs.why-what-how.area-guidance.md` as your template for each file. Below is guidance on what backstory content maps into each section.

**`business-context.md`** — template: area-guidance § business-context.md
- **Problem statement** — queue/error compounding cycle (backstory §2). Two interconnected problems feeding each other.
- **Stakeholders** — table with Mei (owner) and Customers (backstory §4).
- **Background** — 2-year-old shop, loyal following, peak-hour pain (backstory §1).
- **What it IS / What it IS NOT** — self-service touchscreen kiosk augmenting counter service; NOT a staff replacement, NOT mobile ordering, NOT a full POS, NOT a chain management system.
- **How Might We questions** — 3 HMW questions from backstory §12.

**`vision.md`** — template: area-guidance § vision.md
- **Aspiration** — "Every customer gets exactly the drink they ordered, without waiting in a queue." Staff focus on craft.
- **Longer-term direction** — loyalty program, mobile ordering, sales analytics, multi-location.

**`outcomes.md`** — template: area-guidance § outcomes.md
- **MVP definition** — self-service ordering, kitchen display, simulated payment.
- **Success criteria** — order in <2 min, zero customisation errors, unambiguous kitchen display, graceful payment failure.
- **What success does NOT require** — the full exclusion list from backstory §10.

**`scope.md`** — template: area-guidance § scope.md
- **In scope** — ordering (3-drink menu, customisation, review, confirm), payment (card via external provider), kitchen display, order lifecycle, order identification.
- **Out of scope** — table with "What's excluded" and "Why" columns from backstory §11.

**`assumptions-and-risks.md`** — template: area-guidance § assumptions-and-risks.md
- **Assumptions** — 6 items (A1–A6) with status column (all "Assumed") from backstory §13.
- **Risks** — 3 items (R1–R3) with impact and mitigation columns from backstory §13.

**`business.index.md`** — navigation router per `docs.conventions.md` index file conventions.
- Links to business-context, vision, outcomes, scope, assumptions-and-risks.
- Placeholder links for personas/, journeys/, scenarios/ (created in Phase 2).

**`docs.index.md`** — update existing file:
- Replace "XYZ project" with "Compile & Sip".
- Replace "To be populated during Discovery" under Business with links to the files created above.
- Leave Solution, Standards, Decisions as "To be populated" (Phase 3).

## Critical rules

- **NO technology words in any business file.** Do not mention .NET, TUI, console apps, REST, HTTP, APIs, or ASP.NET. The kiosk is always a "self-service touchscreen kiosk." The payment provider is "an external payment provider" — never mention its integration pattern. See backstory §16 Business rules.
- **Business never links into Solution.** Per `docs.conventions.md` linking direction rule.
- **Every file must earn its place.** Per `docs.bootstrap.md` principles. Don't create empty placeholder files.

## Done when

Someone reading business-context, vision, outcomes, scope, and assumptions-and-risks can explain what the project is, who it serves, what it will do, and what it won't — with zero technology leaking through.

**Verify:** Grep all files under `docs/business/` for `.NET|TUI|console app|REST|HTTP|ASP\.NET` — should return zero matches.
