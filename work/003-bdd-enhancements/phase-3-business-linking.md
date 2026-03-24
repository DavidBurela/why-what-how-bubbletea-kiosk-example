# Phase 3: Business Linking

Link verification status from business documentation — scenarios, business context, and business index. This closes the loop: business defines intent, BDD tests verify it, and the documentation shows the proof.

## Context — read these files first

| # | File | What to extract |
|---|------|-----------------|
| 1 | `docs/business/scenarios/scenarios.index.md` | Current table structure — add verification column |
| 2 | `docs/business/business-context.md` | Current content — add verification section at end |
| 3 | `docs/business/business.index.md` | Current navigation — add verification link |
| 4 | `docs/business/scenarios/SCN-001-browse-menu-select-drink.md` | Existing Verification section format — template for updates |
| 5 | `docs/business/scenarios/SCN-002-customise-drink.md` | Same — update Verification section |
| 6 | `docs/business/scenarios/SCN-003-payment-succeeds.md` | Same — update Verification section |
| 7 | `docs/business/scenarios/SCN-004-payment-fails.md` | Same — update Verification section |
| 8 | `docs/business/scenarios/SCN-005-kitchen-marks-order-complete.md` | Same — update Verification section |
| 9 | `docs/docs.conventions.md` | Linking direction rules — Business → BDD tests is allowed (both black-box) |
| 10 | `docs/business/verification/bdd-status.md` | Generated in Phase 2 — target of new links |

## Steps

### 1. Update `docs/business/scenarios/scenarios.index.md`

Add a Verification column to the scenarios table linking to the generated BDD status page:

```markdown
| ID | Name | Journey(s) | Verification | Link |
|----|------|-----------|--------------|------|
| SCN-001 | Browse Menu and Select Drink | JNY-001 | [BDD status](../verification/bdd-status.md) | [SCN-001](SCN-001-browse-menu-select-drink.md) |
| SCN-002 | Customise Drink | JNY-001 | [BDD status](../verification/bdd-status.md) | [SCN-002](SCN-002-customise-drink.md) |
| ... | | | | |
```

### 2. Add `## Verification` section to `docs/business/business-context.md`

Add a new section at the end of the file (before any future appendix sections):

```markdown
## Verification

Business scenarios are verified via executable BDD tests that run against the API boundary. Each scenario defined in [Scenarios](scenarios/scenarios.index.md) has a corresponding feature file containing Given/When/Then examples written in business language.

- [Current verification status](verification/bdd-status.md) — pass/fail summary for all scenarios
- [How BDD verification works](../docs.why-what-how.bdd.md) — methodology and structure
```

This links to both the status (what's passing) and the methodology (how verification works). Both links are same-layer (business → black-box verification) — allowed per linking conventions.

### 3. Enhance `## Verification` in each SCN-xxx scenario file

Each scenario already has a `## Verification` section with a link to its feature file. Add a link to the BDD status page below the existing content. For each of SCN-001 through SCN-005:

```markdown
## Verification

BDD test: `src/tests/CompileAndSip.Bdd.Tests/Features/SCN-xxx-<name>.feature`

Current status: [BDD verification status](../verification/bdd-status.md)
```

Keep the existing feature file path. Add the status link on a new line.

### 4. Update `docs/business/business.index.md`

Add a Verification entry to the navigation. Place it after the "People and journeys" section:

```markdown
## Verification

- [BDD Verification Status](verification/bdd-status.md) — current pass/fail status for all business scenarios
```

## Critical rules

- **No business → solution links.** All new links point to business-layer content (scenarios, verification status, the portable BDD methodology doc). None point into `docs/solution/` or `src/`.
- **Scenario → feature file path is allowed.** This is existing convention (requirement → its proof). Keep it.
- **Scenario → verification status is allowed.** Same principle — business pointing to black-box verification evidence.

## Verification

```bash
# All new links should resolve (check for broken relative paths)
# From docs/business/:
test -f docs/business/verification/bdd-status.md && echo "OK" || echo "FAIL"

# No business files link into solution layer
grep -rn 'solution/' docs/business/business-context.md docs/business/business.index.md docs/business/scenarios/scenarios.index.md
# Should return zero matches (except if "solution" appears in prose — check manually)

# Scenarios index has Verification column
grep -c 'Verification' docs/business/scenarios/scenarios.index.md
# Should return 1+ (header + possibly in content)

# Business context has Verification section
grep -c '## Verification' docs/business/business-context.md
# Should return 1

# Business index has verification link
grep -c 'verification/bdd-status.md' docs/business/business.index.md
# Should return 1

# Each SCN-xxx has the status link
for f in docs/business/scenarios/SCN-*.md; do
  grep -l 'bdd-status.md' "$f" || echo "MISSING: $f"
done
# Should list all 5 files, no MISSING lines

# All tests still pass
cd src && dotnet test && cd ..
```

Then update `work/003-bdd-enhancements/README.md` — change Phase 3 status to "Complete".
