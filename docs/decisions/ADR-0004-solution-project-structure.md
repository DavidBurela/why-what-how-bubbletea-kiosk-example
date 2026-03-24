# ADR-0004: Project Structure — Three Apps with Co-located Tests

**Status:** Accepted

## Context

The Compile & Sip system consists of three applications (Kiosk TUI, Order API, Kitchen Display) as defined in the C4 container diagram. We need a project structure that reflects the architecture clearly while keeping the codebase navigable.

Alternatives considered:

- **Shared Domain library** — single source of truth for domain types, but creates hidden coupling between projects that communicate only via HTTP.
- **Types duplicated in each project** — no shared dependency, but maintenance burden for keeping types in sync.
- **Contracts NuGet package** — versioning overhead unnecessary for a three-project reference example.

## Decision

Three source projects — one per documented application — with the solution file and tests inside `src/`. No shared libraries. TUI apps communicate with the Order API over HTTP only, using their own DTO types.

```
src/
  CompileAndSip.sln
  CompileAndSip.OrderApi/
  CompileAndSip.KioskTui/
  CompileAndSip.KitchenDisplay/
  tests/
    CompileAndSip.OrderApi.Tests/
    CompileAndSip.Bdd.Tests/
    CompileAndSip.KioskTui.Tests/
    CompileAndSip.KitchenDisplay.Tests/
```

## Consequences

- **Architecture alignment** — project count matches the C4 container diagram exactly (3 apps).
- **No hidden coupling** — TUI apps can be understood in isolation. Inter-app communication is HTTP only.
- **Trade-off** — DTOs are duplicated across TUI apps and the API, but they're small and the HTTP boundary is the authoritative contract.
- **Navigability** — readers can ignore `src/` to focus on documentation, or ignore everything else to focus on code.
