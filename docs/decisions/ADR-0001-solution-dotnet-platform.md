# ADR-0001: .NET Platform for All Applications

**Status:** Accepted

## Context

The Compile & Sip Kiosk System requires three applications: a customer-facing ordering terminal (TUI), a central backend API, and a staff-facing kitchen display. The project is a reference example for the Why–What–How documentation framework, so the technology choice must balance demonstrability with simplicity.

Key constraints:

- Everything must run locally with zero external infrastructure.
- The customer terminal and kitchen display need a UI but should not require browser automation for testing.
- The backend needs to serve HTTP endpoints.
- A single technology platform across all applications reduces cognitive overhead for the reference example.

Alternatives considered:

- **Node.js** — strong HTTP ecosystem, but terminal UI libraries are less mature for structured, multi-screen applications.
- **Go** — excellent for CLI tools, but lacks the integrated web framework and TUI library ecosystem.
- **Python** — rapid prototyping, but packaging and distribution for multi-app setups is less straightforward.

## Decision

Use .NET for all three applications:

- **Kiosk TUI** — .NET console application with Terminal UI (Spectre.Console or similar).
- **Order API** — ASP.NET Core minimal API.
- **Kitchen Display** — .NET console application.

## Consequences

- **Single SDK** — contributors need only the .NET SDK installed; no polyglot toolchain.
- **Console apps for UI** — the TUI approach demonstrates user interface concepts without requiring a browser, making BDD testing possible via process interaction.
- **ASP.NET Core minimal API** — lightweight HTTP backend with minimal ceremony.
- **Testing** — .NET's test ecosystem (xUnit, NUnit) integrates naturally with BDD tools (SpecFlow, Reqnroll).
- **Trade-off** — teams unfamiliar with .NET face a learning curve. Mitigated by the project's role as a reference example with documented decisions.
