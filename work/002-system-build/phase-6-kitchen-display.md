# Phase 6: Kitchen Display

Build the staff-facing kitchen order display with HTTP polling. Code in `src/CompileAndSip.KitchenDisplay/`, tests in `src/tests/CompileAndSip.KitchenDisplay.Tests/`.

## Read first

- `docs/solution/applications/kitchen-display/flows/FLW-KDS-001-display-and-complete-orders.md` — flow
- `docs/solution/applications/kitchen-display/app-context.md` — capabilities
- `docs/business/journeys/JNY-002-kitchen-staff-fulfills-order.md` — staff journey
- `docs/business/scenarios/SCN-005-kitchen-marks-order-complete.md` — mark complete behaviour
- `docs/standards/engineering/coding-conventions.md` — DI, interface patterns
- `docs/standards/engineering/api-design.md` — endpoints (GET /orders/active, POST /orders/{id}/complete)

## How it works

```
Poll (GET /orders/active every 2-3s) → Render table → Staff keypresses to complete → POST /orders/{id}/complete → Refresh
```

## Steps

### 1. DTOs — the Kitchen Display's own API contract types

Create `src/CompileAndSip.KitchenDisplay/Dtos.cs` with response types matching the Order API contract:
- `ActiveOrderDto` — order id, order number, drink details, customisation, price, created time

These are local copies of the API contract. The Kitchen Display does NOT reference the OrderApi project.

### 2. API client — separate from rendering

Create `IOrderApiClient` interface:
- `Task<List<ActiveOrderDto>> GetActiveOrdersAsync()`
- `Task<bool> MarkOrderCompleteAsync(Guid orderId)`

Create `OrderApiClient` implementation using `HttpClient`. Base address from configuration.

### 3. Display rendering

Use Spectre.Console to render a table of active orders:
- Header: "Kitchen Display — Compile & Sip"
- Columns: #, Order Number, Drink, Milk, Toppings, Sugar, Ice, Temp, Price
- Orders shown in received order (by order number)
- Footer: instructions for completing orders (e.g., "Press [1-9] to complete an order")

Edge cases:
- No orders → "No active orders — waiting for new orders..."
- API unreachable → "Unable to reach Order API — retrying..."

### 4. Polling loop

- Poll `GET /orders/active` every 2–3 seconds (per ADR-0003)
- Refresh display on each cycle
- Handle API errors gracefully (show message, keep polling)

### 5. Mark complete

- Single keypress to select and complete an order (per SCN-005 — "single action")
- On completion: show brief "Order #XX marked complete!" message
- Display refreshes on next poll cycle

### 6. Program.cs

Wire up with `Host.CreateApplicationBuilder(args)`:
- Register `HttpClient` for `IOrderApiClient` with base address from configuration key `OrderApi:BaseUrl` (default: `http://localhost:5100`)
- Build host, resolve services, start the polling/display loop

Create `src/CompileAndSip.KitchenDisplay/appsettings.json`:

```json
{
  "OrderApi": {
    "BaseUrl": "http://localhost:5100"
  }
}
```

### 7. Tests

Test service/polling logic, NOT Spectre.Console rendering. In `src/tests/CompileAndSip.KitchenDisplay.Tests/`:

| Test | Expected |
|------|----------|
| `GetActiveOrdersAsync_ReturnsOrders` | Deserialized list |
| `GetActiveOrdersAsync_Empty_ReturnsEmptyList` | Empty, not error |
| `MarkOrderCompleteAsync_Success_ReturnsTrue` | true |
| `MarkOrderCompleteAsync_NotFound_ReturnsFalse` | false |

Use a mock HTTP handler or similar technique to test the API client without a running server.

## Verification

```bash
cd src
dotnet test tests/CompileAndSip.KitchenDisplay.Tests --verbosity normal
dotnet build
dotnet test
cd ..
```

## Commit — do this now before moving on

You MUST commit before proceeding to the next phase. Run these commands:

```bash
git add -A
git commit -m "feat(phase-6): implement Kitchen Display with polling"
```

Then update `work/002-system-build/README.md` — change Phase 6 status from "Not started" to "Complete".

Do NOT start Phase 7 until this commit succeeds.
