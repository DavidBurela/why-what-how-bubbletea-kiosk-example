namespace CompileAndSip.OrderApi;

public static class MenuService
{
    private static readonly IReadOnlyList<Drink> Drinks =
    [
        new("classic-milk-tea", "Classic Milk Tea", "Traditional black tea with milk and tapioca pearls. The original.", 5.50m),
        new("taro-milk-tea", "Taro Milk Tea", "Creamy taro root blended with milk tea. Rich and slightly sweet.", 6.00m),
        new("mango-green-tea", "Mango Green Tea", "Fresh mango with jasmine green tea. Light and refreshing.", 6.00m)
    ];

    public static IReadOnlyList<Drink> GetDrinks() => Drinks;

    public static DrinkCustomisation GetDefaultCustomisation() => new();
}
