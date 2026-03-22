# Extension: Area File Guidance

What files each area contains and what should go in them. For the model itself, see `docs.why-what-how.md`. For the folder tree layout, see `docs.structure.md`. For naming conventions, see `docs.conventions.md`.

## Business

### business-context.md

Problem framing and project identity.

Expected sections:

- **Problem statement** — 1–2 paragraphs defining the core problem.
- **Stakeholders** — short table (role, interest). Kept lightweight; promote to a standalone file only when political complexity warrants it.
- **Background** — context that explains why this problem exists now.
- **What it IS / What it IS NOT** — explicit boundaries to prevent misaligned expectations.
- **How Might We questions** — key questions to solve, derived from problem synthesis.

### vision.md

Aspirational direction. Version-agnostic.

Expected sections:

- **Aspiration** — the north-star narrative.
- **Longer-term direction** — where the project could go beyond the first release.

Vision should not contain release-specific objectives or measurable targets — those belong in `outcomes.md`.

### outcomes.md

Versioned, measurable success criteria.

Expected sections:

- **MVP definition** — what the first version specifically delivers.
- **Success criteria** — measurable indicators, specific to a version.
- **What success does NOT require** — explicit exclusions to manage expectations and prevent scope creep.

### scope.md

Boundaries for a specific version.

Expected sections:

- **In scope** — detailed breakdown by category.
- **Partially in scope** — designed for but with limited implementation.
- **Out of scope** — explicitly excluded.

### assumptions-and-risks.md

Assumptions to validate, known risks, and non-negotiable constraints.

Expected sections:

- **Assumptions** — things believed true but not yet validated. Track status (assumed / validated / invalidated) as research progresses.
- **Risks** — known risks with potential impact.
- **Constraints** — non-negotiable environmental, technical, or organisational limits.

Earns its place early — create when initial assumptions emerge during scoping.

### glossary.md

Shared vocabulary. Earns its place when terminology confusion arises across the team or between documents.

### domain-model.md

Domain taxonomy, entity relationships, classifications, or lifecycles. The file name should be project-specific (e.g. `report-model.md`, `care-model.md`).

Earns its place when:

- Multiple documents reference the same set of named entities, states, or categories.
- A glossary entry starts needing a diagram or state machine.

### Personas

Individual persona files under `personas/`.

Expected sections:

- **Who they are** — role and example person.
- **What they care about** — motivations and priorities.
- **What they want from this system** — desired outcomes.
- **Pain points** — current workflow frustrations. Optional; add when research produces them.
- **Decision authority** — can they approve adoption, or do they need someone else? Optional.
- **Key quotes** — verbatim from research. Optional; high-signal for agents and keeps the human voice present.
- **Linked journeys** — references to journey files for this persona.

### Journeys

Individual journey files under `journeys/`.

Expected sections:

- **Persona** — link to the persona file.
- **Goal** — one sentence describing the desired outcome.
- **Trigger** — what starts this journey.
- **Steps** — numbered, narrative sequence.
- **Success criteria** — how to know the journey succeeded.
- **Linked scenarios** — references to scenario files derived from this journey.

### Scenarios

Individual scenario files under `scenarios/`.

Expected sections:

- **Journeys** — links to the journeys this scenario supports.
- **Given / When / Then** — black-box expected behaviour.
- **Notes** — clarifications, edge cases, or additional context.

Scenarios must not link to solution-layer flows (see linking direction rule in `docs.conventions.md`).

## Solution

### solution-context.md

System boundary, external systems, and architecture narrative.

Expected sections:

- **System boundary** — 1–2 paragraphs defining what is inside the solution boundary.
- **External systems** — table (system, role, integration pattern).
- **Architecture narrative** — if using a transition architecture (start simple, evolve toward full vision), explain current state, full vision, and migration path. This is high-signal for agents and new team members.
- **Data flow summary** — a simple diagram or description showing how data moves through the system.

The architecture narrative is particularly valuable in envisioning engagements where you start with a simpler version of the target architecture. Explain what you're building now, what the full picture looks like, and why you're starting with less.

### solution.index.md

Navigation router for the Solution area, plus scenario→flow traceability.

Expected sections:

- **System boundary** — link to `solution-context.md`.
- **Applications** — table of deployable units (name, technology, purpose) with links to application landing pages.
- **Scenario → flow traceability** — table mapping business scenarios to solution flows. Preserves cross-layer traceability in the solution layer where it belongs.

### applications-context.md

Cross-application integration patterns.

Expected sections:

- **Integration pattern** — how applications connect (diagram or description).
- **Relationship details** — per-relationship: which applications, direction, protocol, what flows between them.
- **Future integration** — how relationships will evolve if a transition architecture is in play.

### \<app\>/\<app\>.index.md

Application landing page. Navigation hub for everything about one deployable unit.

Expected sections:

- **Navigation** — links to app-context, components, flows, specs, wireframes as applicable.
- **Related specs** — links to speckit or other specification artifacts.
- **Source code** — path to the application's source code in the repo.

### \<app\>/app-context.md

What this application is, what it depends on, and key decisions that shaped it.

Expected sections:

- **What is this application?** — 1–2 paragraphs.
- **Technology** — stack summary.
- **Capabilities** — table (capability, description).
- **Dependencies** — table (dependency, direction, description). Direction is either inbound or outbound.
- **Key decisions** — links to relevant ADRs.

### Flows

Flows describe internal orchestration (white-box behaviour) for a specific outcome.

They are distinct from business scenarios:

- Scenarios describe what must happen from the outside.
- Flows describe how it happens internally.

Flows live under the relevant application and are canonical in the C4 model (dynamic views). Mermaid sequence diagrams are exported and embedded in markdown.

Expected sections per flow file:

- **Summary** — one paragraph.
- **Participants** — table (participant, role).
- **Sequence** — numbered steps describing the runtime interaction.
- **Scenarios** — links to the business scenarios this flow supports.
- **C4 dynamic view** — reference to the view key in `workspace.dsl`.

See `docs.why-what-how.c4model.md` and `docs.why-what-how.bdd.md` for details on C4 and BDD respectively.

## Standards

File guidance for the Standards area will be expanded as standards are established.

## Decisions

ADRs follow the format and conventions defined in `docs.conventions.md`.
