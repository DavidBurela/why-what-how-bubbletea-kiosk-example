# Phase 4: BDD Verification

Write Reqnroll feature files for business scenarios SCN-001 through SCN-005 and make them pass against the Order API. All tests at the API boundary via WebApplicationFactory — no direct domain calls.

## Read first

- `docs/business/scenarios/SCN-001-browse-menu-select-drink.md`
- `docs/business/scenarios/SCN-002-customise-drink.md`
- `docs/business/scenarios/SCN-003-payment-succeeds.md`
- `docs/business/scenarios/SCN-004-payment-fails.md`
- `docs/business/scenarios/SCN-005-kitchen-marks-order-complete.md`
- `docs/business/menu-model.md` — concrete drink names and the $8.00 example
- `docs/standards/testing/testing-strategy.md` — BDD at API boundary via WebApplicationFactory

## Key conventions

- **Business language only in `.feature` files.** No HTTP verbs, status codes, JSON, or API paths. Write from the customer/staff perspective.
- **One feature file per SCN-xxx.** Named `SCN-xxx-<name>.feature` for traceability.
- **Step definitions are thin HTTP adapters.** They translate Gherkin to HTTP calls — no business logic.
- **Configure payment gateway per scenario** using `Given` steps.

## Steps

### 1. Test infrastructure

Create `src/tests/CompileAndSip.Bdd.Tests/Support/` with:

- **`TestContext`** class — shared per-scenario state holding `HttpClient`, last `HttpResponseMessage`, and `FakePaymentGateway` reference.
- **`Hooks`** class with `[BeforeScenario]` — creates a `WebApplicationFactory<Program>`, configures services, stores `HttpClient` and `FakePaymentGateway` in `TestContext`.
- Register `TestContext` via Reqnroll's dependency injection so step definition classes can receive it via constructor.

### 2. Feature files

Create feature files in `src/tests/CompileAndSip.Bdd.Tests/Features/`:

**`SCN-001-browse-menu-select-drink.feature`** — from SCN-001:
- Customer views the menu
- Sees 3 drinks with names, descriptions, and base prices
- Each drink has a description

**`SCN-002-customise-drink.feature`** — from SCN-002:
- Default customisation gives base price
- Oat milk adds $0.50
- Each topping adds $0.75
- Full customisation (Taro + Oat + 2 toppings) = $8.00
- More than 2 toppings is rejected

**`SCN-003-payment-succeeds.feature`** — from SCN-003:
- Customer places order, payment succeeds
- Receives an order number
- Order is visible on the kitchen display with full details

**`SCN-004-payment-fails.feature`** — from SCN-004:
- Payment declined → friendly error message, order not stored
- Payment unavailable → "pay at counter" message, order not stored

**`SCN-005-kitchen-marks-order-complete.feature`** — from SCN-005:
- Paid order is visible with full customisation details
- Staff marks order complete
- Order disappears from active list

### 3. Step definitions

Create step definition classes in `src/tests/CompileAndSip.Bdd.Tests/StepDefinitions/`:

- **`MenuSteps`** — viewing menu, checking drink names/prices/descriptions
- **`OrderSteps`** — placing orders with customisation
- **`PaymentSteps`** — configuring the fake payment gateway (given steps for success/decline/unavailable)
- **`KitchenSteps`** — viewing active orders, marking complete

Each step class takes `TestContext` via constructor injection. Steps translate Gherkin to HTTP calls (e.g., "the customer views the menu" → `GET /menu`). Use concrete data from menu-model.md in the scenarios.

### 4. Make all scenarios pass

Run the BDD tests. Fix any failing scenarios. Expect ~12+ scenarios total.

### 5. Create BDD conventions standard

Capture the BDD patterns established in this phase.

**`docs/standards/testing/bdd-conventions.md`** — from the patterns used above:
- One `.feature` file per SCN-xxx scenario (e.g., `SCN-001-browse-menu-select-drink.feature`)
- Business language only in Gherkin — no HTTP verbs, status codes, JSON, or API paths
- Step definitions are thin HTTP adapters — translate Gherkin to API calls, no business logic
- Shared `TestContext` via Reqnroll DI for per-scenario state (`HttpClient`, `FakePaymentGateway`)
- `[BeforeScenario]` hook creates `WebApplicationFactory<Program>` fresh per scenario
- Configure payment gateway outcomes via `Given` steps (e.g., "Given the payment will be declined")
- Use concrete data from `docs/business/menu-model.md` in scenarios — real drink names, real prices

## Verification

```bash
cd src

dotnet test tests/CompileAndSip.Bdd.Tests --verbosity normal
# Expect ~12+ scenarios, all passing

# Feature files must use business language only — verify no HTTP terms leaked in:
grep -rn 'GET\|POST\|PUT\|DELETE\|/menu\|/orders\|HTTP\|status.code\|json\|200\|201\|400\|402\|503' tests/CompileAndSip.Bdd.Tests/Features/
# Should return zero matches

# All prior tests still pass
dotnet test

cd ..
find docs/standards -name '*.md' | wc -l   # Should be 8 files
```

## Commit — do this now before moving on

You MUST commit before proceeding to the next phase. Run these commands:

```bash
git add -A
git commit -m "feat(phase-4): add BDD verification for all business scenarios"
```

Then update `work/002-system-build/README.md` — change Phase 4 status from "Not started" to "Complete".

Do NOT start Phase 5 until this commit succeeds.
