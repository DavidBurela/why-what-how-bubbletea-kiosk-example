using CompileAndSip.OrderApi;
using FluentAssertions;

namespace CompileAndSip.OrderApi.Tests;

public sealed class PricingServiceTests
{
    private static readonly Drink ClassicMilkTea = new("classic-milk-tea", "Classic Milk Tea", "Traditional black tea with milk and tapioca pearls. The original.", 5.50m);
    private static readonly Drink TaroMilkTea = new("taro-milk-tea", "Taro Milk Tea", "Creamy taro root blended with milk tea. Rich and slightly sweet.", 6.00m);
    private static readonly Drink MangoGreenTea = new("mango-green-tea", "Mango Green Tea", "Fresh mango with jasmine green tea. Light and refreshing.", 6.00m);

    [Fact]
    public void CalculatePrice_DefaultCustomisation_ReturnsBasePrice()
    {
        var customisation = new DrinkCustomisation();
        var price = PricingService.CalculatePrice(ClassicMilkTea, customisation);
        price.Should().Be(5.50m);
    }

    [Fact]
    public void CalculatePrice_OatMilk_AddsModifier()
    {
        var customisation = new DrinkCustomisation(Milk: MilkType.Oat);
        var price = PricingService.CalculatePrice(ClassicMilkTea, customisation);
        price.Should().Be(6.00m);
    }

    [Fact]
    public void CalculatePrice_NoneMilk_NoModifier()
    {
        var customisation = new DrinkCustomisation(Milk: MilkType.None);
        var price = PricingService.CalculatePrice(ClassicMilkTea, customisation);
        price.Should().Be(5.50m);
    }

    [Fact]
    public void CalculatePrice_OneTopping_AddsCharge()
    {
        var customisation = new DrinkCustomisation(Toppings: [Topping.TapiocaPearls]);
        var price = PricingService.CalculatePrice(TaroMilkTea, customisation);
        price.Should().Be(6.75m);
    }

    [Fact]
    public void CalculatePrice_TwoToppings_AddsCharge()
    {
        var customisation = new DrinkCustomisation(Toppings: [Topping.TapiocaPearls, Topping.CoconutJelly]);
        var price = PricingService.CalculatePrice(TaroMilkTea, customisation);
        price.Should().Be(7.50m);
    }

    [Fact]
    public void CalculatePrice_FullCustomisation_Returns8Dollars()
    {
        var customisation = new DrinkCustomisation(
            Milk: MilkType.Oat,
            Toppings: [Topping.TapiocaPearls, Topping.CoconutJelly]);
        var price = PricingService.CalculatePrice(TaroMilkTea, customisation);
        price.Should().Be(8.00m);
    }

    [Fact]
    public void CalculatePrice_MangoBasePrice_Correct()
    {
        var customisation = new DrinkCustomisation();
        var price = PricingService.CalculatePrice(MangoGreenTea, customisation);
        price.Should().Be(6.00m);
    }
}
