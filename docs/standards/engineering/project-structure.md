# Project Structure

## Solution layout

The solution file (`CompileAndSip.sln`) lives in `src/`. Source projects sit alongside it. Test projects live in `src/tests/`.

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

## Naming convention

All projects use `CompileAndSip.<Name>` naming. Test projects append `.Tests`.

## Source projects

Three source projects — one per documented application:

| Project | Application |
|---------|-------------|
| `CompileAndSip.OrderApi` | Order API (ASP.NET Core Minimal API) |
| `CompileAndSip.KioskTui` | Kiosk TUI (Spectre.Console console app) |
| `CompileAndSip.KitchenDisplay` | Kitchen Display (Spectre.Console console app) |

Each source project folder maps 1:1 to a box on the C4 container diagram.

## Dependency rules

- **Test projects** reference their corresponding source project.
- **TUI apps do NOT reference OrderApi** — they communicate via HTTP only, matching the architecture diagram.
- **Source projects have no inter-project references.**

## Domain types

Domain types (records, enums, value objects) live inside the `CompileAndSip.OrderApi` project. There is no shared library — the API is the domain owner. TUI apps use their own DTOs for HTTP communication.
