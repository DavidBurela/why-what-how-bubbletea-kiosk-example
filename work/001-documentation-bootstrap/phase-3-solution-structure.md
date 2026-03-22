# Phase 3: Solution Structure

Technology becomes explicit. Define the solution boundary, applications, and key architectural decisions.

## Prerequisites

Phases 1 and 2 must be complete. The following should already exist:
- `docs/business/` — business-context, vision, outcomes, scope, assumptions-and-risks, menu
- `docs/business/personas/` — alex.md, jamie.md
- `docs/business/journeys/` — JNY-001, JNY-002
- `docs/business/scenarios/` — SCN-001 through SCN-005

Verify these exist before starting. You will reference them (especially scenarios) from the solution layer.

## Context — read these files first

Read these files in order before creating anything. They provide the source material, templates, and structural rules.

| # | File | What to extract |
|---|------|-----------------|
| 1 | `agent-temp/backstory-compile-and-sip.md` | **§9** (Solution Shape — system boundary, external systems, 3 applications, communication pattern, storage, order tracking), **§14** (What Is Explicitly Simple and Why — rationale for simplicity choices), **§16** (Agent instructions — Solution rules) |
| 2 | `docs/docs.why-what-how.area-guidance.md` | **Solution section** — expected sections for `solution-context.md`, `solution.index.md`, `applications-context.md`, `<app>.index.md`, `app-context.md`. Use these as your templates. |
| 3 | `docs/docs.conventions.md` | **ADR naming** — `ADR-0001-<scope>-<title>.md`. **Scope vocabulary** — `solution`, `application-<name>`. **Application naming** — short, stable folder names. **Flow naming** — `FLW-<APP>-<id>-<name>.md` (needed for traceability table). **Linking direction rule** — solution references business, not the reverse. |
| 4 | `docs/docs.structure.md` | **Target tree** — confirms folder paths (`docs/solution/`, `docs/solution/applications/<app>/`, `docs/decisions/`). **Diagram expectations** — what diagrams go where. **Relationship documentation rule** — define once at lowest correct level, then reference. |
| 5 | `docs/docs.bootstrap.md` | **Phase 3 section** — creation guidance, completion criteria, what to defer. |
| 6 | `work/001-documentation-bootstrap/README.md` | **Decisions D4, D5** — 3 ADRs for key tech decisions; TBD placeholders for source code paths. |

Also read the Phase 1-2 scenario files to understand what business scenarios exist (for the traceability table and flow-to-scenario mappings):
- `docs/business/scenarios/scenarios.index.md` — list of all SCN IDs

## What to create

| # | Action | File | Source from backstory |
|---|--------|------|----------------------|
| 3.1 | Create | `docs/solution/solution.index.md` | Apps table, scenario→flow traceability (flows TBD) |
| 3.2 | Create | `docs/solution/solution-context.md` | §9 system boundary, external systems, architecture narrative |
| 3.3 | Create | `docs/solution/applications/applications-context.md` | §9 integration pattern, per-relationship details |
| 3.4 | Create | `docs/solution/applications/kiosk-tui/kiosk-tui.index.md` | Landing page, short code KSK, source: TBD |
| 3.5 | Create | `docs/solution/applications/kiosk-tui/app-context.md` | .NET console/TUI, capabilities, deps |
| 3.6 | Create | `docs/solution/applications/order-api/order-api.index.md` | Landing page, short code API, source: TBD |
| 3.7 | Create | `docs/solution/applications/order-api/app-context.md` | ASP.NET Core minimal API, capabilities, deps |
| 3.8 | Create | `docs/solution/applications/kitchen-display/kitchen-display.index.md` | Landing page, short code KDS, source: TBD |
| 3.9 | Create | `docs/solution/applications/kitchen-display/app-context.md` | .NET console, capabilities, deps |
| 3.10 | Create | `docs/decisions/decisions.index.md` | ADR catalogue |
| 3.11 | Create | `docs/decisions/ADR-0001-solution-dotnet-platform.md` | Why .NET for all apps |
| 3.12 | Create | `docs/decisions/ADR-0002-application-order-api-in-memory-storage.md` | Why no database |
| 3.13 | Create | `docs/decisions/ADR-0003-solution-rest-http-polling.md` | Why polling over events |
| 3.14 | Update | `docs/docs.index.md` | Add Solution and Decisions section links |

## Content mapping

Use the **expected sections** defined in `docs.why-what-how.area-guidance.md` as your template for each file type. Below is guidance on what backstory content maps into each section.

**`solution-context.md`** — template: area-guidance § solution-context.md
- **System boundary** — Compile & Sip Kiosk System encompasses ordering, payment processing, and kitchen display. From backstory §9.
- **External systems** — table: Payment Gateway (REST API call, simulated). From backstory §9.
- **Architecture narrative** — transition architecture story: starting simple (in-memory storage, single instance, REST/HTTP polling) because this is a reference example. Full vision (loyalty, mobile, analytics, multi-location) would need persistence, authentication, event-driven communication. Rationale from backstory §14.
- **Data flow summary** — the communication pattern diagram from backstory §9: Customer → Kiosk TUI → HTTP → Order API → HTTP → Payment Gateway, with Kitchen Display polling Order API.

**`solution.index.md`** — template: area-guidance § solution.index.md
- **System boundary** — link to solution-context.md.
- **Applications** — table with columns: Application, Short Code, Technology, Purpose. Three rows: Kiosk TUI (KSK, .NET console/TUI), Order API (API, ASP.NET Core minimal API), Kitchen Display (KDS, .NET console). From backstory §9.
- **Scenario → flow traceability** — table mapping SCN-001 through SCN-005 to flows. All flow columns show "—" for now (flows are created in Phase 4). This table preserves cross-layer traceability.

**`applications-context.md`** — template: area-guidance § applications-context.md
- **Integration pattern** — all apps communicate via REST/HTTP. From backstory §9.
- **Relationship details** — per-relationship: Kiosk TUI → Order API (HTTP, submit orders + initiate payment), Order API → Payment Gateway (HTTP, process card payment), Kitchen Display → Order API (HTTP poll, fetch active orders + mark complete). From backstory §9.
- **Future integration** — event-driven could replace polling for kitchen display; mobile app would be additional client of Order API; loyalty service would be new system. From backstory §3 vision items.

**App landing pages** (`<app>.index.md`) — template: area-guidance § \<app\>/\<app\>.index.md
- **Navigation** — link to app-context.md. Flows link placeholder (Phase 4).
- **Source code** — TBD placeholder (e.g., `src/kiosk-tui/` — source code not yet created). Per Decision D5.

**App context files** (`app-context.md`) — template: area-guidance § \<app\>/app-context.md
- **What is this application?** — 1-2 paragraph description from backstory §9.
- **Technology** — stack summary.
- **Capabilities** — table (capability, description).
- **Dependencies** — table (dependency, direction, description). Direction: inbound or outbound.
- **Key decisions** — links to relevant ADRs.

Per-app details from backstory §9:

*Kiosk TUI (KSK):*
- Technology: .NET console application with Terminal UI (Spectre.Console or similar)
- Capabilities: menu display, drink customisation, order review/confirmation, payment initiation, order number display
- Dependencies: Order API (outbound HTTP — submit orders, initiate payment)
- Key decisions: ADR-0001 (platform), ADR-0003 (REST/HTTP)

*Order API (API):*
- Technology: .NET web API (ASP.NET Core minimal API)
- Capabilities: receive orders from kiosk, store orders in memory, process payment via gateway, serve order list to kitchen display, mark orders complete
- Dependencies: Payment Gateway (outbound HTTP), Kiosk TUI (inbound HTTP), Kitchen Display (inbound HTTP)
- Key decisions: ADR-0001 (platform), ADR-0002 (in-memory storage), ADR-0003 (REST/HTTP)

*Kitchen Display (KDS):*
- Technology: .NET console application
- Capabilities: display active orders with full customisation details, mark orders as complete
- Dependencies: Order API (outbound HTTP poll)
- Key decisions: ADR-0001 (platform), ADR-0003 (REST/HTTP polling)

**ADRs** — each follows this structure: Title, Status (Accepted), Context, Decision, Consequences. Per `docs.conventions.md`.

*ADR-0001-solution-dotnet-platform:* Why .NET for all three applications. Context: reference example needs something runnable locally without infrastructure. Alternatives: Node.js, Go, Python. Decision: .NET — console apps for TUI and kitchen display, ASP.NET Core minimal API for the backend. Rationale from backstory §14.

*ADR-0002-application-order-api-in-memory-storage:* Why no database. Context: reference example simplicity — zero infrastructure requirement. Alternatives: SQLite, PostgreSQL. Decision: in-memory storage within Order API process. Orders exist only while the API is running. Rationale from backstory §14.

*ADR-0003-solution-rest-http-polling:* Why REST/HTTP with polling instead of events. Context: simplicity — no message broker, no WebSocket infrastructure. Alternatives: WebSockets/SignalR, message queue (RabbitMQ, etc.). Decision: Kitchen Display polls Order API via HTTP. Rationale from backstory §14.

## Critical rules

- **Solution references business, not the reverse.** Flows and app-context files may reference SCN-xxx scenarios. Business files must NOT be modified to link into solution. Per `docs.conventions.md` linking direction rule.
- **Define relationships at the lowest correct level.** External system deps in `solution-context.md`, cross-app relationships in `applications-context.md`, app-specific deps in `app-context.md`. Reference rather than duplicate. Per `docs.structure.md` relationship documentation rule.
- **ADR scope in filename.** `solution` for cross-cutting decisions, `application-<name>` for app-specific decisions. Per `docs.conventions.md`.
- **Source code paths are TBD.** The source doesn't exist yet. Use placeholder paths.

## Done when

`solution-context.md` and `applications-context.md` make the system boundary, 3 applications, and their connections clear. 3 app landing pages + 3 app-context files exist. 3 ADRs document key technology choices. The scenario→flow traceability table in `solution.index.md` lists all 5 scenarios.

**Verify:**
1. `solution.index.md` lists all 5 scenarios (SCN-001..005) in its traceability table.
2. Each ADR has Title, Status, Context, Decision, Consequences sections.
3. No business files were modified to add solution links.
4. App-context files link to ADRs using correct relative paths.
