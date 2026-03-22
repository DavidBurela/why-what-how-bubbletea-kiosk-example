# Docs Structure (Guidance)

This document describes the intended documentation structure for repositories using the Why–What–How framework.

This file defines **where files live** (the folder tree) and structural rules (diagram placement, relationship deduplication). For **what content goes inside each file** (expected sections, content patterns), see `docs.why-what-how.area-guidance.md`. For the phased creation order, see `docs.bootstrap.md`.

The tree below is the target layout. Not every project needs every file from day one. When making changes, prefer moving toward this layout and documenting exceptions as needed.

If you propose a new long-lived folder pattern, update this file so the structure remains easy to navigate for humans and agents.

## Target tree

```text
docs/
  docs.index.md

  docs.structure.md
  docs.why-what-how.md
  docs.why-what-how.area-guidance.md
  docs.why-what-how.c4model.md
  docs.why-what-how.bdd.md
  docs.bootstrap.md
  docs.conventions.md

  business/
    business.index.md
    business-context.md
    vision.md
    outcomes.md
    scope.md
    assumptions-and-risks.md
    glossary.md

    personas/
      personas.index.md
      <persona>.md
    journeys/
      journeys.index.md
      JNY-<id>-<name>.md
    scenarios/
      scenarios.index.md
      SCN-<id>-<name>.md

  solution/
    solution.index.md
    solution-context.md

    c4-model/
      workspace.dsl
      export/
        *.mmd

    applications/
      applications-context.md

      <application-name>/
        <application-name>.index.md
        app-context.md
        components/
          <component-name>.md
        flows/
          FLW-<APP>-<id>-<name>.md

  standards/
    standards.index.md
    engineering/
    observability/
    security/
    ui-ux/
    delivery/

  decisions/
    decisions.index.md
    ADR-0001-<scope>-<title>.md
```

## Area responsibilities

- `business/`
  Intent and black-box expectations. No internal orchestration.
- `solution/`
  White-box structure and design. Applications, components, flows, and key relationships. The C4 model lives here.
- `standards/`
  Cross-cutting constraints and guardrails.
- `decisions/`
  ADRs that preserve reasoning and trade-offs.

## Diagram expectations

Diagrams are exported from the C4 model and embedded in markdown at the relevant level.

- Solution level (`solution/`)
  - A system context diagram showing the solution boundary and external systems.
  - Key external relationships called out below the diagram in short bullet form.

- Applications level (`solution/applications/`)
  - An applications relationship diagram showing how applications connect.
  - Key cross-application relationships called out below the diagram in short bullet form.

- Application level (`solution/applications/<application-name>/`)
  - Optional component diagram for that application (useful when decomposition is non-trivial).
  - App context that summarises inbound and outbound dependencies.

- Flows level (`solution/applications/<application-name>/flows/`)
  - A sequence diagram exported from the C4 dynamic view.
  - Flow notes that explain orchestration concerns (contracts, retries, idempotency, telemetry) at a high level.

## Relationship documentation rule

Define relationships once at the lowest correct level, then reference rather than duplicate.

Examples:

- External system dependencies are defined at the solution level (`solution-context.md`), then referenced from applications as needed.
- Cross-application links are defined at the applications level (`applications-context.md`), then referenced from `app-context.md`.
