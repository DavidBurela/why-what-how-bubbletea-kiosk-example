# Extension: C4 Model (Structurizr) and Diagram Exports

This document explains how the C4 model supports the Solution layer.

## Purpose

The C4 model is used to:

- Define the structural graph (elements and relationships)
- Define dynamic views for flows
- Validate structural integrity
- Export Mermaid diagrams for embedding into markdown

## Location

- Model: `docs/solution/c4-model/workspace.dsl`
- Generated exports: `docs/solution/c4-model/export/`

Exports are generated artifacts. Do not edit exported `.mmd` files manually.

## Core separation

- C4 `workspace.dsl` is the structural source of truth for elements and relationships.
- Solution markdown is the explanatory source of truth (intent, rationale, details).
- Exported Mermaid diagrams are derived from the C4 model.

## What the model includes

- System context (solution boundary and external systems)
- Applications (deployable units) and their relationships
- Components (internal responsibility boundaries) where useful
- Dynamic views for flows (sequence-style orchestration)

## Validation

Use the Structurizr toolchain to validate:

- Elements exist
- Relationships are valid
- Views reference valid elements
- Dynamic views reference valid participants

Validation is structural. It confirms consistency, not correctness of intent.

## Exporting diagrams to Mermaid

Export Mermaid diagrams periodically, then embed them in the relevant markdown files:

- `docs/solution/solution-context.md`
- `docs/solution/applications/applications-context.md`
- Application component docs if used
- Flow docs under `flows/`

## Generated content rule

If a markdown file contains a clearly marked generated section (for example, a Mermaid block that is stated to be generated), treat that section as derived output and overwrite it via export, not manual editing.
