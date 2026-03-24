using CompileAndSip.OrderApi;
using FluentAssertions;

namespace CompileAndSip.OrderApi.Tests;

public sealed class MenuServiceTests
{
    [Fact]
    public void GetDrinks_ReturnsExactlyThree()
    {
        var drinks = MenuService.GetDrinks();
        drinks.Should().HaveCount(3);
    }

    [Fact]
    public void GetDrinks_ClassicMilkTea_CorrectData()
    {
        var drinks = MenuService.GetDrinks();
        var classic = drinks.Single(d => d.Id == "classic-milk-tea");
        classic.Name.Should().Be("Classic Milk Tea");
        classic.Description.Should().Be("Traditional black tea with milk and tapioca pearls. The original.");
        classic.BasePrice.Should().Be(5.50m);
    }

    [Fact]
    public void GetDrinks_TaroMilkTea_CorrectData()
    {
        var drinks = MenuService.GetDrinks();
        var taro = drinks.Single(d => d.Id == "taro-milk-tea");
        taro.Name.Should().Be("Taro Milk Tea");
        taro.Description.Should().Be("Creamy taro root blended with milk tea. Rich and slightly sweet.");
        taro.BasePrice.Should().Be(6.00m);
    }

    [Fact]
    public void GetDrinks_MangoGreenTea_CorrectData()
    {
        var drinks = MenuService.GetDrinks();
        var mango = drinks.Single(d => d.Id == "mango-green-tea");
        mango.Name.Should().Be("Mango Green Tea");
        mango.Description.Should().Be("Fresh mango with jasmine green tea. Light and refreshing.");
        mango.BasePrice.Should().Be(6.00m);
    }

    [Fact]
    public void GetDefaultCustomisation_MatchesDefaults()
    {
        var defaults = MenuService.GetDefaultCustomisation();
        defaults.Milk.Should().Be(MilkType.Regular);
        defaults.Toppings.Should().BeEmpty();
        defaults.Sugar.Should().Be(SugarLevel.Hundred);
        defaults.Ice.Should().Be(IceLevel.RegularIce);
        defaults.Temperature.Should().Be(Temperature.Cold);
    }
}
