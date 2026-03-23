# Phase 2: Domain Model (TDD)

Implement shared domain types, menu data, pricing, and order model — all test-first. Code in `src/CompileAndSip.Domain/`, tests in `tests/CompileAndSip.Domain.Tests/`.

## Read first

- `docs/business/menu-model.md` — **canonical source** for drinks, prices, customisation, and the $8.00 example
- `docs/business/scenarios/SCN-002-customise-drink.md` — customisation options and defaults
- `docs/standards/engineering/coding-conventions.md` — records, enums, file-scoped namespaces

## Reference data (from menu-model.md)

### Drinks

| Drink | Base Price | Description |
|-------|-----------|-------------|
| Classic Milk Tea | $5.50 | Traditional black milk tea with a smooth, creamy finish |
| Taro Milk Tea | $6.00 | Creamy taro root blended with milk tea |
| Mango Green Tea | $6.00 | Refreshing green tea with tropical mango |

### Customisation and pricing

| Category | Options | Default | Price impact |
|----------|---------|---------|-------------|
| Milk | Regular, Oat, None | Regular | Oat: +$0.50, others: $0 |
| Toppings | Tapioca Pearls, Coconut Jelly, Pudding (pick 0–2) | None | +$0.75 each |
| Sugar | 0%, 25%, 50%, 75%, 100% | 100% | None |
| Ice | No Ice, Less Ice, Regular Ice | Regular Ice | None |
| Temperature | Hot, Cold | Cold | None |

**Price formula:** Base + milk modifier + (topping count × $0.75)

**Key verification:** Taro Milk Tea ($6.00) + Oat milk ($0.50) + 2 toppings ($1.50) = **$8.00**

## Steps

TDD workflow: write failing test → write minimum code to pass → refactor. Repeat for each component.

### 1. Enums

Create enums: `MilkType` (Regular, Oat, None), `Topping` (TapiocaPearls, CoconutJelly, Pudding), `SugarLevel` (Zero, TwentyFive, Fifty, SeventyFive, Hundred), `IceLevel` (NoIce, LessIce, RegularIce), `Temperature` (Hot, Cold), `OrderStatus` (Created, Paid, Complete).

### 2. Records

- `Drink(string Id, string Name, string Description, decimal BasePrice)`
- `DrinkCustomisation` — with properties for each category, defaults matching the table above (Regular milk, empty topping list, 100% sugar, Regular ice, Cold)
- `OrderItem(Drink Drink, DrinkCustomisation Customisation, decimal CalculatedPrice)`

### 3. PricingService (static class)

`CalculatePrice(Drink drink, DrinkCustomisation customisation) → decimal`

**Write these tests first:**

| Test name | Input | Expected |
|-----------|-------|----------|
| `CalculatePrice_DefaultCustomisation_ReturnsBasePrice` | Classic Milk Tea, defaults | $5.50 |
| `CalculatePrice_OatMilk_AddsModifier` | Classic Milk Tea, Oat milk | $6.00 |
| `CalculatePrice_NoneMilk_NoModifier` | Classic Milk Tea, None | $5.50 |
| `CalculatePrice_OneTopping_AddsCharge` | Taro Milk Tea, 1 topping | $6.75 |
| `CalculatePrice_TwoToppings_AddsCharge` | Taro Milk Tea, 2 toppings | $7.50 |
| `CalculatePrice_FullCustomisation_Returns8Dollars` | Taro, Oat, 2 toppings | **$8.00** |
| `CalculatePrice_MangoBasePrice_Correct` | Mango Green Tea, defaults | $6.00 |

### 4. MenuService (static class)

`GetDrinks() → IReadOnlyList<Drink>` — returns exactly 3 drinks matching the table above.
`GetDefaultCustomisation() → DrinkCustomisation` — returns the defaults.

**Write these tests first:**

| Test name | Expected |
|-----------|----------|
| `GetDrinks_ReturnsExactlyThree` | Count = 3 |
| `GetDrinks_ClassicMilkTea_CorrectData` | Name, description, $5.50 |
| `GetDrinks_TaroMilkTea_CorrectData` | Name, description, $6.00 |
| `GetDrinks_MangoGreenTea_CorrectData` | Name, description, $6.00 |
| `GetDefaultCustomisation_MatchesDefaults` | Regular milk, no toppings, 100% sugar, Regular ice, Cold |

### 5. Order entity (class, not record — has mutable status)

Properties: `Id` (Guid), `OrderNumber` (int), `Item` (OrderItem), `Status` (OrderStatus), `CreatedAt` (DateTimeOffset).

State transitions:
- `MarkPaid()` — Created → Paid. Throws `InvalidOperationException` from any other state.
- `MarkComplete()` — Paid → Complete. Throws `InvalidOperationException` from any other state.

**Write these tests first:**

| Test name | Expected |
|-----------|----------|
| `NewOrder_HasCreatedStatus` | Status = Created |
| `MarkPaid_FromCreated_Succeeds` | Status = Paid |
| `MarkComplete_FromPaid_Succeeds` | Status = Complete |
| `MarkComplete_FromCreated_Throws` | InvalidOperationException |
| `MarkPaid_FromPaid_Throws` | InvalidOperationException |

### 6. OrderValidator (static class)

`Validate(string drinkId, DrinkCustomisation customisation) → List<string>` — returns validation errors (empty = valid).

Rules: drink ID must exist in menu, toppings count 0–2, no duplicate toppings.

**Write these tests first:**

| Test name | Expected |
|-----------|----------|
| `Validate_ValidOrder_NoErrors` | Empty list |
| `Validate_UnknownDrink_ReturnsError` | 1 error |
| `Validate_ThreeToppings_ReturnsError` | 1 error |
| `Validate_DuplicateToppings_ReturnsError` | 1 error |

### 7. IPaymentGateway and PaymentResult

- `IPaymentGateway` interface with `Task<PaymentResult> ProcessPaymentAsync(decimal amount)`
- `PaymentResult` record with `bool Succeeded`, `string? ErrorMessage`, `bool GatewayUnavailable`

No tests for the interface itself — `FakePaymentGateway` is tested in Phase 3.

### 8. Create testing standards

Now that we've established TDD patterns, capture them as standards for future phases.

**`docs/standards/testing/testing-strategy.md`** — from the verification approach used in this build:
- Three layers: BDD (Reqnroll, API boundary) catches intent errors, TDD (xUnit + FluentAssertions) catches implementation errors, manual E2E via Aspire catches integration errors
- BDD tests run against the API via `WebApplicationFactory` — no external process needed
- Console/TUI rendering is NOT tested — only service layers and API clients
- All tests must be deterministic — no random data, no time-dependent assertions

**`docs/standards/testing/tdd-conventions.md`** — from the patterns used in steps 1–7 above:
- Red-green-refactor: write failing test first, implement minimum to pass, then refactor
- Test naming: `MethodName_Condition_ExpectedResult`
- Arrange-Act-Assert structure
- Use FluentAssertions for all assertions (`.Should().Be()`, `.Should().Contain()`, etc.)
- Test data must match `docs/business/menu-model.md` — use the real drink names and prices

## Verification

```bash
dotnet test tests/CompileAndSip.Domain.Tests --verbosity normal
# Expect ~20+ tests, all passing
dotnet build
find docs/standards -name '*.md' | wc -l   # Should be 6 files
```

## Commit — do this now before moving on

You MUST commit before proceeding to the next phase. Run these commands:

```bash
git add -A
git commit -m "feat: implement domain model with TDD and testing standards"
```

Then update `work/002-system-build/README.md` — change Phase 2 status from "Not started" to "Complete".

Do NOT start Phase 3 until this commit succeeds.
