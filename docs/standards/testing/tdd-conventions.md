# TDD Conventions

## Workflow

Red-green-refactor:

1. **Red** — Write a failing test first.
2. **Green** — Write the minimum code to make the test pass.
3. **Refactor** — Clean up while keeping tests green.

## Test naming

`MethodName_Condition_ExpectedResult`

Examples:
- `CalculatePrice_DefaultCustomisation_ReturnsBasePrice`
- `MarkPaid_FromCreated_Succeeds`
- `Validate_UnknownDrink_ReturnsError`

## Test structure

Arrange-Act-Assert (AAA):

```csharp
[Fact]
public void MethodName_Condition_ExpectedResult()
{
    // Arrange
    var input = ...;

    // Act
    var result = SystemUnderTest.Method(input);

    // Assert
    result.Should().Be(expected);
}
```

## Assertions

Use FluentAssertions for all assertions:

- `.Should().Be()` for exact values
- `.Should().HaveCount()` for collection sizes
- `.Should().BeEmpty()` for empty collections
- `.Should().Throw<T>()` for expected exceptions
- `.Should().Contain()` for substring or collection membership

## Test data

Test data must match `docs/business/menu-model.md` — use the real drink names and prices. Do not invent fictitious data for domain tests.
