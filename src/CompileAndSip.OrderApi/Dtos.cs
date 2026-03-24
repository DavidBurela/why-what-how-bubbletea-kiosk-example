namespace CompileAndSip.OrderApi;

public record DrinkCustomisationDto(
    string? Milk = null,
    List<string>? Toppings = null,
    string? Sugar = null,
    string? Ice = null,
    string? Temperature = null);

public record OrderRequest(string DrinkId, DrinkCustomisationDto? Customisation = null);

public record OrderResponse(Guid Id, int OrderNumber, decimal TotalPrice);

public record DrinkDto(string Id, string Name, string Description, decimal BasePrice);

public record CustomisationOptionsDto(
    string[] MilkOptions,
    string[] ToppingOptions,
    string[] SugarLevels,
    string[] IceLevels,
    string[] Temperatures,
    DrinkCustomisationDto Defaults);

public record MenuResponse(IReadOnlyList<DrinkDto> Drinks, CustomisationOptionsDto CustomisationOptions);

public record ActiveOrderDto(
    Guid Id,
    int OrderNumber,
    string DrinkName,
    DrinkCustomisationDto Customisation,
    decimal TotalPrice,
    DateTimeOffset CreatedAt);

public record ErrorResponse(string Error);
