namespace CompileAndSip.OrderApi;

public record Drink(string Id, string Name, string Description, decimal BasePrice);

public record DrinkCustomisation(
    MilkType Milk = MilkType.Regular,
    IReadOnlyList<Topping>? Toppings = null,
    SugarLevel Sugar = SugarLevel.Hundred,
    IceLevel Ice = IceLevel.RegularIce,
    Temperature Temperature = Temperature.Cold)
{
    public IReadOnlyList<Topping> Toppings { get; init; } = Toppings ?? [];
}

public record OrderItem(Drink Drink, DrinkCustomisation Customisation, decimal CalculatedPrice);
