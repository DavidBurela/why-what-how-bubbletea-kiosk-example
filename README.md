# Bubble Tea Kiosk — A Why-What-How Example

A reference implementation of the [Why-What-How](https://github.com/DavidBurela/why-what-how) documentation framework, using a bubble tea ordering kiosk as a realistic worked example.

## What is this?

This repository demonstrates how the **Why-What-How** framework organises project knowledge — from business intent through solution architecture to architectural decisions.

The scenario is **Compile & Sip**, an independent bubble tea shop introducing a self-service ordering kiosk to solve two real problems: long queues during peak hours and order customisation errors from verbal relay. The system spans three applications — a customer-facing kiosk, a central order API, and a kitchen display — giving enough architectural surface to showcase the framework without unnecessary complexity.

## The Scenario

Compile & Sip is a neighbourhood bubble tea shop where peak-hour queues cause 20–30% of potential customers to walk away, and verbal order relay leads to wrong milk types, forgotten toppings, and wasted drinks. The solution is a self-service kiosk where customers browse a three-drink menu, customise their order (milk type, toppings, sugar, ice, temperature), and pay by card. Orders appear instantly on a kitchen display for preparation, eliminating the verbal bottleneck entirely.

Two personas drive the design: **Alex** (a regular customer who values accuracy and speed) and **Jamie** (a kitchen staff member who needs clear, unambiguous order details).

Read more: [Business Context](docs/business/business-context.md) · [Vision](docs/business/vision.md) · [Personas](docs/business/personas/personas.index.md)

## What the Framework Covers

| Area | Frame | What's in this repo |
|------|-------|---------------------|
| **Business** | Why | Vision, outcomes, scope, menu model, 2 personas, 2 journeys, 5 testable scenarios |
| **Solution** | What | System context, 3 applications (Kiosk TUI, Order API, Kitchen Display), application flows, C4 model |
| **Standards** | How | Engineering conventions, testing strategy, local development |
| **Decisions** | Why&nbsp;This | 7 ADRs: .NET platform, in-memory storage, REST/HTTP polling, project structure, test strategy, manual orchestration, simulated payment |

## Repository Structure

```
docs/
├── docs.index.md                      # Master navigation
├── docs.why-what-how.md               # Framework overview
├── docs.bootstrap.md                  # How to bootstrap your own project
├── docs.conventions.md                # Naming and linking conventions
├── docs.structure.md                  # Folder structure reference
│
├── business/                          # WHY — intent, people, behaviour
│   ├── business-context.md            #   Problem framing and stakeholders
│   ├── vision.md                      #   Aspirational direction
│   ├── outcomes.md                    #   Success criteria
│   ├── scope.md                       #   In/out-of-scope boundaries
│   ├── menu-model.md                  #   Drink catalogue and pricing
│   ├── assumptions-and-risks.md       #   Assumptions to validate
│   ├── personas/                      #   Alex (customer), Jamie (kitchen staff)
│   ├── journeys/                      #   JNY-001, JNY-002
│   └── scenarios/                     #   SCN-001 through SCN-005
│
├── solution/                          # WHAT — structure, decomposition
│   ├── solution-context.md            #   System boundary and architecture
│   ├── applications/                  #   Per-application context and flows
│   │   ├── kiosk-tui/                 #     Customer-facing ordering terminal
│   │   ├── order-api/                 #     Central backend service
│   │   └── kitchen-display/           #     Staff-facing order display
│   └── c4-model/                      #   Structurizr workspace (DSL + JSON)
│
└── decisions/                         # WHY THIS — architectural decision records
    ├── ADR-0001-solution-dotnet-platform.md
    ├── ADR-0002-application-order-api-in-memory-storage.md
    ├── ADR-0003-solution-rest-http-polling.md
    ├── ADR-0004-solution-project-structure.md
    ├── ADR-0005-solution-test-strategy.md
    ├── ADR-0006-solution-manual-orchestration.md
    └── ADR-0007-application-order-api-simulated-payment-gateway.md
```

## How to Explore

A suggested reading order for newcomers:

1. **Understand the framework** — [docs.why-what-how.md](docs/docs.why-what-how.md)
2. **Read the business context** — [business-context.md](docs/business/business-context.md) and [vision.md](docs/business/vision.md)
3. **Meet the users** — [Personas](docs/business/personas/personas.index.md) → [Journeys](docs/business/journeys/journeys.index.md) → [Scenarios](docs/business/scenarios/scenarios.index.md)
4. **See the solution shape** — [solution-context.md](docs/solution/solution-context.md) and [applications-context.md](docs/solution/applications/applications-context.md)
5. **Understand the trade-offs** — [Decisions](docs/decisions/decisions.index.md)
6. **Bootstrap your own** — [docs.bootstrap.md](docs/docs.bootstrap.md)

Or start from the [docs index](docs/docs.index.md) and navigate freely.

## About the Why-What-How Framework

The [Why-What-How](https://github.com/DavidBurela/why-what-how) framework organises project knowledge into four areas:

- **Business (Why)** — Define intent and external behaviour: who we serve, what problems exist, what outcomes matter
- **Solution (What)** — Define structural decomposition: what we're building, how it breaks down, what owns what
- **Standards (How)** — Constrain implementation for consistency: conventions, patterns, quality bars
- **Decisions (Why This)** — Preserve reasoning: why we chose this approach, what alternatives existed, what trade-offs we accepted

The structure supports **progressive context loading** — both humans and AI agents can navigate from high-level intent down to specific detail without needing to read everything at once.

## How to Run

**Prerequisites:** .NET 10 SDK

**Quick start:**

```bash
cd src
dotnet build
dotnet test
```

See [src/README.md](src/README.md) for full instructions including how to start all three applications.

## Status

This repository includes both the documentation layer and a complete implementation:

- Business, Solution, Standards, and Decisions areas are fully populated
- Source code: 3 applications + 4 test projects in `src/`
- 62 tests: domain TDD, API integration, BDD scenarios, and service layer tests
- C4 diagrams included in the workspace

## License

This work is licensed under [CC BY 4.0](LICENSE).
