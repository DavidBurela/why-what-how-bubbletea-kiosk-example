# Conventions

This document defines naming, IDs, linking rules, ADR scope vocabulary, and generated content conventions.

## Naming conventions

### Journeys

- File: `docs/business/journeys/JNY-<id>-<name>.md`

### Scenarios

- File: `docs/business/scenarios/SCN-<id>-<name>.md`

### Flows

- File: `docs/solution/applications/<application-name>/flows/FLW-<APP>-<id>-<name>.md`

`<APP>` is a short code (typically 3 letters) for the owning application. Each application defines its own code in its landing page. IDs are sequential per application.

Flows describe white-box orchestration. They are canonical as C4 dynamic views and exported to Mermaid for embedding.

Each flow document should include a `**Level:**` field indicating the C4 abstraction level for its sequence diagram:

- `context` — interactions between the system and external actors
- `container` — interactions across applications (deployable units)
- `component` — interactions within a single application

Flows always live under their owning application regardless of level.

### ADRs

- File: `docs/decisions/ADR-0001-<scope>-<title>.md`

Scope is included in the filename to make applicability clear.

Recommended scope vocabulary (evolving):

- `solution`
- `application-<application-name>`
- `standards`

## Index file conventions

Index files exist at two levels:

1. **Docs root and core area roots** (mandatory):
   - `docs/docs.index.md` — entry point and router for the entire docs tree.
   - `docs/business/business.index.md`
   - `docs/solution/solution.index.md`
   - `docs/standards/standards.index.md`
   - `docs/decisions/decisions.index.md`

2. **Application landing pages and catalogues of numbered items** (where metadata beyond filenames is needed):
   - Application landing pages: `<application-name>/<application-name>.index.md` — bridges docs, specs, and source code.
   - Catalogues: `journeys/journeys.index.md`, `scenarios/scenarios.index.md`, `personas/personas.index.md` — provide metadata tables (persona, status, journey mappings) that filenames alone cannot convey.

If a folder has a handful of descriptively named files, it does not need an index. Agents can read the folder listing directly.

Index files are navigation routers. They contain links and short descriptions, not narrative content.

## Linking conventions

### Linking direction rule

Business never points into Solution. The implementer references the requirement, not the other way around.

- Within the same layer (e.g. journey ↔ scenario), link freely in both directions.
- Cross-layer, only the solution layer references the business layer.

| From                | To               | Allowed | Reason                                                  |
| ------------------- | ---------------- | ------- | ------------------------------------------------------- |
| Journey → Scenario  | down             | Yes     | Same layer (both business, black-box)                   |
| Scenario → Journey  | up               | Yes     | Same layer. Scenario says which journeys it belongs to. |
| Scenario → BDD test | down             | Yes     | Both black-box. Requirement points to its proof.        |
| Flow → Scenario     | up (cross-layer) | Yes     | Solution references business.                           |
| Scenario → Flow     | cross-layer      | **No**  | Business must not reference solution internals.         |
| Journey → Flow      | cross-layer      | **No**  | Same reason.                                            |

### Journey links to scenarios

Each journey should include a short list of scenario IDs it relies on.

### Scenario links to verification

Each scenario should include:

- Link(s) to BDD test path(s) that verify the scenario (feature files)

Scenarios are black-box and must not link to solution-layer flows. Traceability from scenarios to flows is maintained in the solution layer (see `solution.index.md`).

### Flow links back to scenarios

Each flow doc should link back to the scenario(s) it supports. This is the correct direction for cross-layer traceability: solution references business, not the other way around.

## Black box vs white box (reminder)

- Journeys and Scenarios: black box definition
- BDD tests: black box validation
- Solution structure and Flows: white box definition
- C4 tooling: white box structural validation
- Unit tests/TDD: white box logical validation

## Generated artifacts and generated blocks

### Exported diagrams

- Location: `docs/solution/c4-model/export/`
- These files are generated. Do not edit them manually.

### Generated sections in markdown

If a markdown file contains a clearly marked generated section, treat it as derived output.
Update the source (usually the C4 model) and regenerate, rather than editing the generated block.

Suggested marker pattern:

- `<!-- BEGIN GENERATED: <description> -->`
- `<!-- END GENERATED -->`

Agents must overwrite the content between these markers entirely — do not merge or append. Treat the markers as a regeneration boundary.

## Application and component naming

Applications:

- Represent deployable boundaries
- Prefer short, stable folder names that match model element names

Components:

- Represent stable responsibility boundaries within an application
- Features come and go; components endure
- Example component names:
  - `orders`
  - `reporting`
  - `authentication`
  - `notifications`
