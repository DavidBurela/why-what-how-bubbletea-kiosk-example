using CompileAndSip.OrderApi;
using FluentAssertions;

namespace CompileAndSip.OrderApi.Tests;

public sealed class OrderValidatorTests
{
    [Fact]
    public void Validate_ValidOrder_NoErrors()
    {
        var customisation = new DrinkCustomisation(Toppings: [Topping.TapiocaPearls]);
        var errors = OrderValidator.Validate("classic-milk-tea", customisation);
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_UnknownDrink_ReturnsError()
    {
        var customisation = new DrinkCustomisation();
        var errors = OrderValidator.Validate("unknown-drink", customisation);
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Validate_ThreeToppings_ReturnsError()
    {
        var customisation = new DrinkCustomisation(
            Toppings: [Topping.TapiocaPearls, Topping.CoconutJelly, Topping.Pudding]);
        var errors = OrderValidator.Validate("classic-milk-tea", customisation);
        errors.Should().HaveCount(1);
    }

    [Fact]
    public void Validate_DuplicateToppings_ReturnsError()
    {
        var customisation = new DrinkCustomisation(
            Toppings: [Topping.TapiocaPearls, Topping.TapiocaPearls]);
        var errors = OrderValidator.Validate("classic-milk-tea", customisation);
        errors.Should().HaveCount(1);
    }
}
