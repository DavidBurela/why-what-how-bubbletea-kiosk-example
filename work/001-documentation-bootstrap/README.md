# Documentation Bootstrap — Compile & Sip

Populate the Why-What-How documentation framework for the Compile & Sip bubble tea kiosk using the backstory (`agent-temp/backstory-compile-and-sip.md`) as primary source. Four phases, each executed in a separate session.

## How to execute

Each phase is designed as a **self-contained agent prompt** — it lists the files to read for context, what to create, content mapping, rules, and verification steps. Start a fresh session for each phase to reclaim token budget.

### Session prompt template

Paste this into a new session, replacing `<phase-file>` with the path:

```
Read `work/001-documentation-bootstrap/<phase-file>` and execute it.

Follow the "Context — read these files first" section to load background before creating anything. Then create all listed files following the content mapping and critical rules. Run the verification checks at the end.
```

### Execution order

| Phase | Prompt file | Status |
|-------|-------------|--------|
| 1 | `work/001-documentation-bootstrap/phase-1-business-foundation.md` | Not started |
| 2 | `work/001-documentation-bootstrap/phase-2-who-and-why.md` | Not started |
| 3 | `work/001-documentation-bootstrap/phase-3-solution-structure.md` | Not started |
| 4 | `work/001-documentation-bootstrap/phase-4-depth.md` | Not started |

Review the output of each phase before starting the next. Each phase file lists prerequisites from prior phases.

## Decisions

| # | Decision | Rationale |
|---|----------|-----------|
| D1 | Menu data → `docs/business/menu.md` as a domain document | The drink catalogue, customisation options, and pricing rules are black-box domain knowledge referenced by scope, scenarios, and journeys. Project-specific name per area-guidance convention. |
| D2 | SCN-004 variant (gateway unavailable) → Notes section within SCN-004 | Per backstory: "may be captured as a separate BDD test rather than a separate scenario." |
| D3 | `assumptions-and-risks.md` included in Phase 1 | 6 assumptions + 3 risks from backstory §13 earn its place. Bootstrap guide says "Often created early in Phase 1." |
| D4 | 3 ADRs cover key tech decisions | Platform (.NET), storage (in-memory), communication (REST/HTTP polling) — per backstory §16 guidance. |
| D5 | Source code paths in app landing pages → TBD placeholders | The landing page template expects a source code path. Source doesn't exist yet. |
| D6 | Phase 4 is conditional | Only attempted if user confirms Phases 1-3 are solid. |
| D7 | Glossary deferred to Phase 4 | 7 terms from backstory §15 — create only if multiple docs reference them and clarity is needed. |

## Verification Checklist

| # | Check | When |
|---|-------|------|
| V1 | Grep `docs/business/` for `.NET\|TUI\|console app\|REST\|HTTP\|ASP\.NET` → zero matches | After Phase 1, 2 |
| V2 | No file under `docs/business/` links into `docs/solution/` | After Phase 2 |
| V3 | JNY-001, JNY-002 cross-referenced from persona files | After Phase 2 |
| V4 | SCN-001..005 cross-referenced from journey files | After Phase 2 |
| V5 | `solution.index.md` lists all 5 scenarios in traceability table | After Phase 3 |
| V6 | Each ADR has Title, Status, Context, Decision, Consequences | After Phase 3 |
| V7 | Each file has expected sections per area-guidance | After each phase |
| V8 | All IDs follow naming conventions (JNY-xxx, SCN-xxx, FLW-APP-xxx, ADR-xxxx) | After each phase |
