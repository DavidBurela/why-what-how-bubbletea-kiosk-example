# Phase 3: Order API (TDD)

Implement the ASP.NET Core Minimal API with in-memory storage and simulated payment — all test-first. Code in `src/CompileAndSip.OrderApi/`, tests in `src/tests/CompileAndSip.OrderApi.Tests/`.

## Read first

- `docs/solution/applications/order-api/flows/FLW-API-001-process-order-and-payment.md` — order processing flow
- `docs/solution/applications/order-api/app-context.md` — API capabilities
- `docs/business/scenarios/SCN-003-payment-succeeds.md` — success path
- `docs/business/scenarios/SCN-004-payment-fails.md` — failure paths
- `docs/standards/engineering/coding-conventions.md` — DI, ConcurrentDictionary, minimal API patterns
- `docs/standards/testing/tdd-conventions.md` — test-first, naming, FluentAssertions

## API contract

| Method | Path | Success | Errors |
|--------|------|---------|--------|
| GET | `/menu` | 200 — drinks + customisation options | — |
| POST | `/orders` | 201 — `{ id, orderNumber, totalPrice }` | 400 validation, 402 declined, 503 unavailable |
| GET | `/orders/active` | 200 — array of active orders | — |
| POST | `/orders/{id}/complete` | 200 | 404 not found |

Error shape: `{ "error": "<message>" }`

### POST /orders flow (from FLW-API-001)

1. Receive request with `drinkId` and customisation
2. Validate (drink exists, toppings 0–2, no duplicates) → 400 if invalid
3. Calculate price via `PricingService`
4. Process payment via `IPaymentGateway` → 402 if declined, 503 if unavailable
5. On success: assign sequential order number, store order, return 201

**Critical: if payment fails, do NOT store the order.**

## Steps

TDD workflow: write failing integration test (HttpClient via WebApplicationFactory) → implement → refactor.

### 1. FakePaymentGateway

Implements `IPaymentGateway` (created in Phase 2). Lives in the `src/CompileAndSip.OrderApi/` project.

- Default behaviour: always succeeds
- `ConfigureNextResult(PaymentResult)` — lets tests set the next payment outcome

**Tests:**

| Test | Expected |
|------|----------|
| `DefaultBehaviour_Succeeds` | PaymentResult.Succeeded = true |
| `ConfiguredDecline_Fails` | Succeeded = false, has error message |
| `ConfiguredUnavailable_Fails` | GatewayUnavailable = true |

### 2. IOrderStore + InMemoryOrderStore

Interface: `Add(Order)`, `GetById(Guid)`, `GetActiveOrders()` (returns Paid orders), `NextOrderNumber()`.

Implementation: `ConcurrentDictionary<Guid, Order>`, `Interlocked.Increment` for order numbers.

**Tests:**

| Test | Expected |
|------|----------|
| `Add_ThenGetById_ReturnsOrder` | Same order returned |
| `GetActiveOrders_ReturnsPaidOnly` | Excludes Created and Complete |
| `NextOrderNumber_IsSequential` | 1, 2, 3... |

### 3. DTOs (in the OrderApi project)

- `OrderRequest` — `string DrinkId`, `DrinkCustomisationDto` (milk, toppings, sugar, ice, temperature)
- `OrderResponse` — `Guid Id`, `int OrderNumber`, `decimal TotalPrice`
- `MenuResponse` — drinks list with customisation options and defaults
- `ActiveOrderDto` — order number, drink details, customisation, price, created time
- `ErrorResponse` — `string Error`

Map between DTOs and domain types in the API layer.

### 4. DI registration and Program.cs

Update `src/CompileAndSip.OrderApi/Program.cs`:
- `IPaymentGateway` → `FakePaymentGateway` (register as singleton, also register concrete type for test access)
- `IOrderStore` → `InMemoryOrderStore` (singleton)
- Ensure `public partial class Program { }` at bottom for test access

### 5. Implement endpoints

Implement all 4 endpoints matching the contract table above. Keep handlers thin — delegate to domain services.

**Integration tests (all via WebApplicationFactory + HttpClient):**

| Test | Expected |
|------|----------|
| `GetMenu_Returns200WithThreeDrinks` | 200, 3 drinks |
| `PostOrder_ValidOrder_Returns201` | 201, has order number + price |
| `PostOrder_ValidOrder_AppearsInActive` | GET /orders/active includes it |
| `PostOrder_SequentialNumbers` | Order numbers increment |
| `PostOrder_PaymentDeclined_Returns402` | 402, error message |
| `PostOrder_PaymentDeclined_NotStored` | GET /orders/active is empty |
| `PostOrder_GatewayUnavailable_Returns503` | 503, error message |
| `PostOrder_GatewayUnavailable_NotStored` | GET /orders/active is empty |
| `PostOrder_InvalidDrink_Returns400` | 400, validation error |
| `PostOrder_TooManyToppings_Returns400` | 400, validation error |
| `GetActiveOrders_Empty_Returns200` | 200, empty array |
| `PostComplete_PaidOrder_Returns200` | 200, order no longer active |
| `PostComplete_NotFound_Returns404` | 404 |

### 6. WebApplicationFactory test setup

Create a base test fixture or helper that:
- Boots the API via `WebApplicationFactory<Program>`
- Provides `HttpClient` for requests
- Exposes `FakePaymentGateway` so tests can configure payment outcomes per test

### 7. Create API design standard

Capture the API conventions as a standard for consistency.

**`docs/standards/engineering/api-design.md`** — from the endpoint contract and FLW-API-001:
- Endpoint table: GET /menu, POST /orders, GET /orders/active, POST /orders/{id}/complete
- Status codes: 200 success, 201 created, 400 validation error, 402 payment declined, 404 not found, 503 gateway unavailable
- Error shape: `{ "error": "<message>" }` — consistent JSON object, not a bare string
- JSON only — no XML, no content negotiation
- DTOs are the HTTP contract — domain types are internal to the OrderApi project
- Handlers are thin — delegate to domain services for business logic

## Verification

```bash
cd src
dotnet test tests/CompileAndSip.OrderApi.Tests --verbosity normal
# Expect ~20+ tests (domain + API), all passing
dotnet build
cd ..
find docs/standards -name '*.md' | wc -l   # Should be 7 files
```

## Commit — do this now before moving on

You MUST commit before proceeding to the next phase. Run these commands:

```bash
git add -A
git commit -m "feat(phase-3): implement Order API endpoints with TDD"
```

Then update `work/002-system-build/README.md` — change Phase 3 status from "Not started" to "Complete".

Do NOT start Phase 4 until this commit succeeds.
