# Phase 6: Kitchen Display

Build the staff-facing kitchen order display with HTTP polling. Code in `src/CompileAndSip.KitchenDisplay/`, tests in `tests/CompileAndSip.KitchenDisplay.Tests/`.

## Read first

- `docs/solution/applications/kitchen-display/flows/FLW-KDS-001-display-and-complete-orders.md` — flow
- `docs/solution/applications/kitchen-display/app-context.md` — capabilities
- `docs/business/journeys/JNY-002-kitchen-staff-fulfills-order.md` — staff journey
- `docs/business/scenarios/SCN-005-kitchen-marks-order-complete.md` — mark complete behaviour
- `docs/standards/engineering/coding-conventions.md` — DI, interface patterns
- `docs/standards/engineering/api-design.md` — endpoints (GET /orders/active, POST /orders/{id}/complete)
- `docs/standards/delivery/local-development.md` — Aspire service discovery pattern

## How it works

```
Poll (GET /orders/active every 2-3s) → Render table → Staff keypresses to complete → POST /orders/{id}/complete → Refresh
```

## Steps

### 1. API client — separate from rendering

Create `IOrderApiClient` interface:
- `Task<List<ActiveOrderDto>> GetActiveOrdersAsync()`
- `Task<bool> MarkOrderCompleteAsync(Guid orderId)`

Create `OrderApiClient` implementation using `HttpClient`. Base address from Aspire service discovery (`https+http://order-api`).

### 2. Display rendering

Use Spectre.Console to render a table of active orders:
- Header: "Kitchen Display — Compile & Sip"
- Columns: #, Order Number, Drink, Milk, Toppings, Sugar, Ice, Temp, Price
- Orders shown in received order (by order number)
- Footer: instructions for completing orders (e.g., "Press [1-9] to complete an order")

Edge cases:
- No orders → "No active orders — waiting for new orders..."
- API unreachable → "Unable to reach Order API — retrying..."

### 3. Polling loop

- Poll `GET /orders/active` every 2–3 seconds (per ADR-0003)
- Refresh display on each cycle
- Handle API errors gracefully (show message, keep polling)

### 4. Mark complete

- Single keypress to select and complete an order (per SCN-005 — "single action")
- On completion: show brief "Order #XX marked complete!" message
- Display refreshes on next poll cycle

### 5. Program.cs

Wire up with `Host.CreateApplicationBuilder(args)`:
- Call `builder.AddServiceDefaults()`
- Register `HttpClient` for `IOrderApiClient` with base address `https+http://order-api`
- Build host, resolve services, start the polling/display loop

### 6. Tests

Test service/polling logic, NOT Spectre.Console rendering. In `tests/CompileAndSip.KitchenDisplay.Tests/`:

| Test | Expected |
|------|----------|
| `GetActiveOrdersAsync_ReturnsOrders` | Deserialized list |
| `GetActiveOrdersAsync_Empty_ReturnsEmptyList` | Empty, not error |
| `MarkOrderCompleteAsync_Success_ReturnsTrue` | true |
| `MarkOrderCompleteAsync_NotFound_ReturnsFalse` | false |

Use a mock HTTP handler or similar technique to test the API client without a running server.

## Verification

```bash
dotnet test tests/CompileAndSip.KitchenDisplay.Tests --verbosity normal
dotnet build
dotnet test
```

## Commit — do this now before moving on

You MUST commit before proceeding to the next phase. Run these commands:

```bash
git add -A
git commit -m "feat: implement Kitchen Display with polling and Spectre.Console"
```

Then update `work/002-system-build/README.md` — change Phase 6 status from "Not started" to "Complete".

Do NOT start Phase 7 until this commit succeeds.
