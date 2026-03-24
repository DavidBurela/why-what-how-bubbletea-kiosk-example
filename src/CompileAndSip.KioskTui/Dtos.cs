namespace CompileAndSip.KioskTui;

public record DrinkDto(string Id, string Name, string Description, decimal BasePrice);

public record CustomisationOptionsDto(
    string[] MilkOptions,
    string[] ToppingOptions,
    string[] SugarLevels,
    string[] IceLevels,
    string[] Temperatures,
    DrinkCustomisationDto Defaults);

public record MenuResponse(IReadOnlyList<DrinkDto> Drinks, CustomisationOptionsDto CustomisationOptions);

public record DrinkCustomisationDto(
    string? Milk = null,
    List<string>? Toppings = null,
    string? Sugar = null,
    string? Ice = null,
    string? Temperature = null);

public record OrderRequest(string DrinkId, DrinkCustomisationDto? Customisation = null);

public record OrderResponse(Guid Id, int OrderNumber, decimal TotalPrice);

public record ErrorResponse(string Error);
