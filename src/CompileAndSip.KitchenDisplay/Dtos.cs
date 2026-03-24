namespace CompileAndSip.KitchenDisplay;

public record DrinkCustomisationDto(
    string? Milk = null,
    List<string>? Toppings = null,
    string? Sugar = null,
    string? Ice = null,
    string? Temperature = null);

public record ActiveOrderDto(
    Guid Id,
    int OrderNumber,
    string DrinkName,
    DrinkCustomisationDto Customisation,
    decimal TotalPrice,
    DateTimeOffset CreatedAt);
