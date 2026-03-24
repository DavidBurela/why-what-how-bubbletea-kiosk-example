# Phase 5: Kiosk TUI

Build the customer-facing Spectre.Console terminal app. Code in `src/CompileAndSip.KioskTui/`, tests in `src/tests/CompileAndSip.KioskTui.Tests/`.

## Read first

- `docs/solution/applications/kiosk-tui/flows/FLW-KSK-001-customer-places-order.md` — full screen flow
- `docs/solution/applications/kiosk-tui/app-context.md` — capabilities
- `docs/business/journeys/JNY-001-customer-places-order.md` — customer journey steps
- `docs/business/scenarios/SCN-004-payment-fails.md` — payment failure UX (preserve customisation)
- `docs/standards/engineering/coding-conventions.md` — DI, interface patterns
- `docs/standards/engineering/api-design.md` — endpoints the kiosk calls (GET /menu, POST /orders)

## Screen flow (from FLW-KSK-001)

```
Home → Menu → Customise → Review → Pay → Confirmation
                                      ↓ (failure)
                                    Error → Retry/Cancel
```

## Steps

### 1. DTOs — the Kiosk's own API contract types

Create `src/CompileAndSip.KioskTui/Dtos.cs` with request/response types matching the Order API contract:
- `MenuResponse`, `DrinkDto`, `CustomisationOptionsDto`
- `OrderRequest`, `DrinkCustomisationDto`
- `OrderResponse` (id, orderNumber, totalPrice)
- `ErrorResponse`

These are the Kiosk's local copies of the API contract. They do NOT reference the OrderApi project — the HTTP boundary is the only contract.

### 2. API client — separate from rendering

Create `IOrderApiClient` interface:
- `Task<MenuResponse> GetMenuAsync()`
- `Task<OrderResult> PlaceOrderAsync(OrderRequest request)`

Create `OrderApiClient` implementation using `HttpClient`. Base address from configuration.

`OrderResult` should distinguish: success (with order number + price), payment declined, gateway unavailable.

### 3. Screens (6 — following the flow)

Use Spectre.Console prompts and rendering. Each screen is a method or small class.

**Home** — Welcome message ("Welcome to Compile & Sip!"), press any key to start.

**Menu** — `SelectionPrompt` showing 3 drinks with name, description, and base price. Customer selects one.

**Customise** — Series of prompts:
- Milk: SelectionPrompt (Regular, Oat +$0.50, None)
- Toppings: MultiSelectionPrompt, 0–2 from 3 options (+$0.75 each)
- Sugar: SelectionPrompt (0%, 25%, 50%, 75%, 100%)
- Ice: SelectionPrompt (No Ice, Less Ice, Regular Ice)
- Temperature: SelectionPrompt (Hot, Cold)
- Show running price total after selections

**Review** — Display a table with drink name, all customisation choices, and total price. Confirm or go back to menu.

**Payment** — Show "Processing payment..." with a brief simulated pause, call `PlaceOrderAsync()`.

**Confirmation/Error** —
- Success: "Your order number is #XX" with the total price
- Declined: "Payment didn't go through — try again?" (retry preserves customisation per SCN-004)
- Unavailable: "Payment temporarily unavailable. Please pay at the counter."

### 4. Program.cs

Wire up with `Host.CreateApplicationBuilder(args)`:
- Register `HttpClient` for `IOrderApiClient` with base address from configuration key `OrderApi:BaseUrl` (default: `http://localhost:5100`)
- Build the host, resolve the API client, run the screen flow loop
- The app should loop back to Home after each order (or exit on Ctrl+C)

Create `src/CompileAndSip.KioskTui/appsettings.json`:

```json
{
  "OrderApi": {
    "BaseUrl": "http://localhost:5100"
  }
}
```

### 5. Tests

Test the service layer, NOT the Spectre.Console rendering. In `src/tests/CompileAndSip.KioskTui.Tests/`:

| Test | Expected |
|------|----------|
| `GetMenuAsync_ReturnsMenu` | Deserialized menu with 3 drinks |
| `PlaceOrderAsync_Success_ReturnsOrderNumber` | OrderResult with number + price |
| `PlaceOrderAsync_Declined_ReturnsError` | OrderResult indicating decline |
| `PlaceOrderAsync_Unavailable_ReturnsUnavailable` | OrderResult indicating unavailable |

Use a mock HTTP handler or similar technique to test the API client without a running server.

## Verification

```bash
cd src
dotnet test tests/CompileAndSip.KioskTui.Tests --verbosity normal
dotnet build
dotnet test
cd ..
```

## Commit — do this now before moving on

You MUST commit before proceeding to the next phase. Run these commands:

```bash
git add -A
git commit -m "feat(phase-5): implement Kiosk TUI with Spectre.Console"
```

Then update `work/002-system-build/README.md` — change Phase 5 status from "Not started" to "Complete".

Do NOT start Phase 6 until this commit succeeds.
