# Bootstrap Guide

How to set up the Why–What–How documentation framework for a new project.

This is a creation playbook — it tells you what to create, in what order, and what questions to answer at each phase. For the model itself, see `docs.why-what-how.md`. For expected sections within each file, see `docs.why-what-how.area-guidance.md`. For naming and linking rules, see `docs.conventions.md`. For the target folder layout, see `docs.structure.md`.

## Principles

- Start from business intent, not solution structure.
- Create the minimum files needed at each phase. Add depth as the project matures.
- Every file should earn its place — if you can't fill it with real content, defer it.
- Prefer one good document over three thin ones.

## Phase 1 — Foundation

Establish the project's identity and boundaries. This is the minimum to orient anyone (human or agent) on what the project is and why it exists.

### Create

| File                                | Purpose                                                                   | Key questions to answer                                                  |
| ----------------------------------- | ------------------------------------------------------------------------- | ------------------------------------------------------------------------ |
| `docs/docs.index.md`                | Entry point and navigation router                                         | Skeleton — fill in as areas are populated.                               |
| `docs/business/business.index.md`   | Business area navigation                                                  | Links to files below.                                                    |
| `docs/business/business-context.md` | Problem framing, stakeholders, what it is / isn't, how-might-we questions | What problem are we solving? Who are the stakeholders? What is this NOT? |
| `docs/business/vision.md`           | Aspirational direction (version-agnostic)                                 | What does the north-star look like? Where could this go long term?       |
| `docs/business/outcomes.md`         | Versioned success criteria, MVP definition                                | How will we know it worked? What does success NOT require?               |
| `docs/business/scope.md`            | In/out/partial scope boundaries                                           | What's in scope for the first release? What's explicitly out?            |

### Also create (framework files)

Copy the `docs.*.md` framework files into the new project:

- `docs.why-what-how.md`
- `docs.why-what-how.area-guidance.md`
- `docs.why-what-how.bdd.md`
- `docs.why-what-how.c4model.md`
- `docs.structure.md`
- `docs.conventions.md`
- `docs.bootstrap.md` (this file)

These are project-agnostic and work as-is. The only file that must be project-specific from the start is `docs.index.md`.

### Defer

- Glossary — add when terminology confusion arises.
- Assumptions and risks (`assumptions-and-risks.md`) — add when initial assumptions emerge during scoping. Often created early in Phase 1 or shortly after.
- Domain model — add when domain taxonomy is non-trivial. Use a project-specific name (e.g. `process-model.md`, `report-model.md`).
- Everything in solution, standards, and decisions.

### Phase 1 is complete when

Someone reading `business-context.md`, `vision.md`, `outcomes.md`, and `scope.md` can explain what the project is, who it serves, what it will do, and what it won't.

---

## Phase 2 — Who and why

Define who uses the system and what they need. This phase makes business intent concrete and testable.

### Create

| File                                             | Purpose                               | Key questions to answer                                             |
| ------------------------------------------------ | ------------------------------------- | ------------------------------------------------------------------- |
| `docs/business/personas/personas.index.md`       | Persona catalogue with priority tiers | Who is the primary persona? Who is secondary?                       |
| `docs/business/personas/<persona>.md` (1+)       | Individual persona                    | What do they care about? What do they want from this system?        |
| `docs/business/journeys/journeys.index.md`       | Journey catalogue                     | —                                                                   |
| `docs/business/journeys/JNY-001-<name>.md` (1+)  | End-to-end user journey               | What does the primary persona do, step by step? What triggers this? |
| `docs/business/scenarios/scenarios.index.md`     | Scenario catalogue                    | —                                                                   |
| `docs/business/scenarios/SCN-001-<name>.md` (1+) | Black-box Given/When/Then             | What must be true from the outside?                                 |

### Guidance

- Start with **one journey for the primary persona**. Derive scenarios from it.
- Scenarios should be testable without knowing the solution architecture.
- Don't force completeness — capture the scenarios you can articulate now.
- Journeys link to scenarios. Scenarios link to journeys. (Same layer, bidirectional.)
- Persona files can be enriched over time with pain points, decision authority, and key quotes. These fields are optional at creation — add them when research produces the evidence.

### Defer

- BDD test files — write them when you have code to test against.
- Additional personas and journeys — add as the scope expands.

### Phase 2 is complete when

The primary persona has at least one journey with derived scenarios, and those scenarios would make sense to someone who knows nothing about the technical solution.

---

## Phase 3 — Solution structure

Define what you're building. This happens once business intent from Phases 1–2 is clear enough to decompose.

### Create

| File                                                 | Purpose                                 | Key questions to answer                                                        |
| ---------------------------------------------------- | --------------------------------------- | ------------------------------------------------------------------------------ |
| `docs/solution/solution.index.md`                    | Solution area navigation + traceability | Applications, cross-app context link, scenario→flow mapping.                   |
| `docs/solution/solution-context.md`                  | System boundary and external systems    | What is inside the solution boundary? What external systems does it depend on? |
| `docs/solution/applications/applications-context.md` | Cross-application relationships         | How do the applications connect? What's the integration pattern?               |
| `docs/solution/applications/<app>/<app>.index.md`    | Application landing page                | What does this app do? Where's its source code?                                |
| `docs/solution/applications/<app>/app-context.md`    | Inbound/outbound dependencies           | What does this app consume and expose?                                         |
| `docs/decisions/decisions.index.md`                  | ADR catalogue                           | —                                                                              |

### Guidance

- Identify **deployable units** (applications) first. Each gets a folder.
- Define relationships between applications at the `applications-context.md` level.
- Record the first ADR if a significant architectural choice has been made.
- Add the scenario→flow traceability table to `solution.index.md` (even with "—" for flows not yet written).
- If starting with a transition architecture (simpler than the full vision), use `solution-context.md` to tell the architecture story: what you're building now, what the full picture looks like, and why you're starting with less.

### Defer

- Flows — add when runtime orchestration needs explaining.
- Components — add when an application is complex enough to decompose.
- C4 model (`workspace.dsl`) — add when structural validation becomes valuable.
- Standards — seed later from ADRs or team conventions.

### Phase 3 is complete when

Someone can look at `solution-context.md` and `applications-context.md` and understand the system boundary, the major building blocks, and how they connect.

---

## Phase 4 — Depth

Add detail as the project matures. These are not mandatory; add each when it earns its place.

| What                  | When to add                                                   | Where                                                        |
| --------------------- | ------------------------------------------------------------- | ------------------------------------------------------------ |
| Assumptions and risks | When initial assumptions emerge during scoping                | `business/assumptions-and-risks.md`                          |
| Domain model          | When domain taxonomy is referenced by multiple documents      | `business/domain-model.md` (project-specific name)           |
| Flows                 | When a runtime interaction needs explaining beyond prose      | `solution/applications/<app>/flows/FLW-<APP>-<id>-<name>.md` |
| C4 model              | When structural validation or generated diagrams are valuable | `solution/c4-model/workspace.dsl`                            |
| Standards             | When conventions need to be enforced consistently             | `standards/<area>/`                                          |
| BDD tests             | When scenarios need executable verification                   | Application test folders (not under `docs/`)                 |
| Glossary              | When terminology confusion arises                             | `business/glossary.md`                                       |
| Component docs        | When an application's internals are complex                   | `solution/applications/<app>/components/`                    |

### Standards bootstrapping tip

A good first pass at standards is to scan accepted ADRs and extract recurring constraints (languages, hosting, security patterns, observability expectations).

---

## Populating from Design Thinking methods

If using Design Thinking methods for discovery, the table below maps method outputs to business folder files. DT working artifacts stay in scratch space (e.g. `.copilot-tracking/dt/`); distil durable outcomes into the business folder when they become canonical.

| DT Method                   | What it produces                   | Distils into                                                                    |
| --------------------------- | ---------------------------------- | ------------------------------------------------------------------------------- |
| **M1: Scope Conversations** | Stakeholder map                    | `business-context.md` — stakeholder table                                       |
|                             | Scope boundaries                   | `scope.md`                                                                      |
|                             | Assumptions log                    | `assumptions-and-risks.md`                                                      |
| **M2: Design Research**     | Interview quotes and pain points   | `personas/` — pain points, key quotes                                           |
|                             | Workflow observations              | `journeys/` — journey steps                                                     |
|                             | Unmet needs across user groups     | `scenarios/` — Given/When/Then slices                                           |
|                             | Environmental constraints          | `assumptions-and-risks.md` — constraints                                        |
|                             | Assumption validation results      | `assumptions-and-risks.md` — status updates                                     |
| **M3: Input Synthesis**     | Problem definition                 | `business-context.md` — problem statement refinement                            |
|                             | Insight statements                 | `personas/` — pain points; `business-context.md` — framing                      |
|                             | How-might-we questions             | `business-context.md` — HMW section                                             |
|                             | Validated themes → success signals | `outcomes.md` — success criteria                                                |
| **M6: Lo-Fi Prototypes**    | Constraint discoveries             | `assumptions-and-risks.md` — new constraints, validated/invalidated assumptions |

Method 2 is the richest feeder into the business folder — it populates personas, journeys, scenarios, and assumptions. Method 1 sets up the framing. Method 3 refines and fills gaps.

Later methods (M4–M5) produce working artifacts that stay in scratch space. Method 6 feeds constraint discoveries back into `assumptions-and-risks.md`. Method 7 (hi-fi prototypes) is the natural trigger for creating Phase 3 solution docs — technical architecture requirements map directly to `solution-context.md`, `applications-context.md`, and ADRs.

---

## Quick reference — minimum viable docs

For the smallest useful documentation set, complete Phases 1 and 2:

```text
docs/
  docs.index.md
  docs.why-what-how.md
  docs.conventions.md
  docs.structure.md
  docs.bootstrap.md

  business/
    business.index.md
    business-context.md
    vision.md
    outcomes.md
    scope.md
    # assumptions-and-risks.md — add when assumptions emerge
    personas/
      personas.index.md
      <primary-persona>.md
    journeys/
      journeys.index.md
      JNY-001-<name>.md
    scenarios/
      scenarios.index.md
      SCN-001-<name>.md
```

Everything else is added incrementally based on need.
