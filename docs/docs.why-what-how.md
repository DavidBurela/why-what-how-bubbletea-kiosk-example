# Why–What–How

This repository structures engagement knowledge using four areas. Three define the system — mapped to the Why–What–How framing — and one captures reasoning:

- Business defines intent. (Why)
- Solution defines structure. (What)
- Standards define constraints. (How)
- Decisions preserve reasoning. (Why This — a cross-cutting layer, not a phase)

This model is designed to be:

- Simple for humans to navigate
- Useful for agents via progressive, level-appropriate context loading
- Clear about what belongs where

## The Four Areas

### Business (Why)

Business captures:

- Problem context and framing
- Vision (aspirational, version-agnostic)
- Outcomes (versioned, measurable)
- Scope
- Assumptions and risks
- Personas
- Journeys (end-to-end user narrative)
- Scenarios (expected behaviour, black box)

Business answers:

- Why does this exist?
- Who are we serving?
- What outcomes matter?
- What must the system do from the outside?

Business does not describe internal orchestration or implementation details.

### Solution (What)

Solution captures:

- System context and boundaries
- Applications (deployable units)
- Components (stable responsibility boundaries inside an application)
- Relationships between elements
- Flows (white-box orchestration for specific outcomes)

Solution answers:

- What are we building?
- How is it decomposed?
- What owns what?
- How do parts interact?
- How does a specific interaction unfold internally?

### Standards (How)

Standards capture cross-cutting constraints:

- Engineering conventions
- Observability expectations
- Security patterns
- UI/UX rules
- Delivery practices

Standards answer:

- How do we build consistently?
- What conventions and patterns do we follow?

### Decisions (Why This)

Decisions preserve reasoning using ADRs:

- Why we chose an approach
- Alternatives considered
- Trade-offs accepted
- Scope of impact

---

## Extensions

- `docs.why-what-how.area-guidance.md` — what each file should contain (expected sections, content patterns).
- `docs.why-what-how.bdd.md` — journeys, scenarios, and BDD testing.
- `docs.why-what-how.c4model.md` — C4 model and diagram exports.
