namespace CompileAndSip.OrderApi;

public static class OrderValidator
{
    public static List<string> Validate(string drinkId, DrinkCustomisation customisation)
    {
        var errors = new List<string>();

        var drinks = MenuService.GetDrinks();
        if (!drinks.Any(d => d.Id == drinkId))
            errors.Add($"Unknown drink: '{drinkId}'.");

        if (customisation.Toppings.Count > 2)
            errors.Add("Maximum 2 toppings allowed.");

        if (customisation.Toppings.Distinct().Count() != customisation.Toppings.Count)
            errors.Add("Duplicate toppings are not allowed.");

        return errors;
    }
}
