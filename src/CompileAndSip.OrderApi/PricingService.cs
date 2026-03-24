namespace CompileAndSip.OrderApi;

public static class PricingService
{
    private const decimal OatMilkModifier = 0.50m;
    private const decimal ToppingCharge = 0.75m;

    public static decimal CalculatePrice(Drink drink, DrinkCustomisation customisation)
    {
        var price = drink.BasePrice;

        if (customisation.Milk == MilkType.Oat)
            price += OatMilkModifier;

        price += customisation.Toppings.Count * ToppingCharge;

        return price;
    }
}
