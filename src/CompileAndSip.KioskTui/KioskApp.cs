using Spectre.Console;

namespace CompileAndSip.KioskTui;

public sealed class KioskApp
{
    private readonly IOrderApiClient _apiClient;

    public KioskApp(IOrderApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ShowHomeScreen();
            var menu = await _apiClient.GetMenuAsync();
            var drink = ShowMenuScreen(menu);
            var customisation = ShowCustomiseScreen(drink, menu.CustomisationOptions);
            var totalPrice = CalculateDisplayPrice(drink, customisation);

            if (!ShowReviewScreen(drink, customisation, totalPrice))
                continue;

            var request = new OrderRequest(drink.Id, customisation);
            var result = await ShowPaymentScreen(request);

            ShowResultScreen(result, customisation, drink, request);
        }
    }

    private static void ShowHomeScreen()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Compile & Sip").Color(Color.Green));
        AnsiConsole.MarkupLine("[bold]Welcome to Compile & Sip![/]");
        AnsiConsole.MarkupLine("Press [green]any key[/] to start ordering...");
        Console.ReadKey(true);
    }

    private static DrinkDto ShowMenuScreen(MenuResponse menu)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold underline]Our Menu[/]\n");

        var prompt = new SelectionPrompt<DrinkDto>()
            .Title("Select a drink:")
            .UseConverter(d => $"{d.Name} — ${d.BasePrice:F2} — {d.Description}");

        foreach (var drink in menu.Drinks)
            prompt.AddChoice(drink);

        return AnsiConsole.Prompt(prompt);
    }

    private static DrinkCustomisationDto ShowCustomiseScreen(DrinkDto drink, CustomisationOptionsDto options)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[bold]Customise your {drink.Name}[/]\n");

        var milk = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose milk type:")
                .AddChoices(FormatMilkOptions(options.MilkOptions)));
        var milkValue = ExtractMilkValue(milk);

        var toppings = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Choose toppings (0-2, space to select):")
                .NotRequired()
                .AddChoices(FormatToppingOptions(options.ToppingOptions)));
        var toppingValues = toppings.Select(ExtractToppingValue).ToList();

        // Enforce max 2 toppings
        if (toppingValues.Count > 2)
        {
            AnsiConsole.MarkupLine("[red]Maximum 2 toppings allowed. Taking first 2.[/]");
            toppingValues = toppingValues.Take(2).ToList();
        }

        var sugar = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Sugar level:")
                .AddChoices(options.SugarLevels));

        var ice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Ice level:")
                .AddChoices(options.IceLevels));

        var temperature = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Temperature:")
                .AddChoices(options.Temperatures));

        return new DrinkCustomisationDto(milkValue, toppingValues, sugar, ice, temperature);
    }

    private static bool ShowReviewScreen(DrinkDto drink, DrinkCustomisationDto customisation, decimal totalPrice)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[bold underline]Order Review[/]\n");

        var table = new Table();
        table.AddColumn("Item");
        table.AddColumn("Details");

        table.AddRow("Drink", drink.Name);
        table.AddRow("Milk", customisation.Milk ?? "Regular");
        table.AddRow("Toppings", customisation.Toppings?.Count > 0 ? string.Join(", ", customisation.Toppings) : "None");
        table.AddRow("Sugar", customisation.Sugar ?? "Hundred");
        table.AddRow("Ice", customisation.Ice ?? "RegularIce");
        table.AddRow("Temperature", customisation.Temperature ?? "Cold");
        table.AddRow("[bold]Total Price[/]", $"[bold green]${totalPrice:F2}[/]");

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        return AnsiConsole.Confirm("Confirm and proceed to payment?");
    }

    private async Task<OrderResult> ShowPaymentScreen(OrderRequest request)
    {
        AnsiConsole.Clear();
        return await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .StartAsync("Processing payment...", async _ =>
            {
                await Task.Delay(500); // Brief simulated pause
                return await _apiClient.PlaceOrderAsync(request);
            });
    }

    private static void ShowResultScreen(OrderResult result, DrinkCustomisationDto customisation, DrinkDto drink, OrderRequest request)
    {
        AnsiConsole.Clear();
        switch (result.Status)
        {
            case OrderResultStatus.Success:
                AnsiConsole.Write(new Panel(
                    $"[bold green]Your order number is #{result.OrderNumber}[/]\n" +
                    $"Total: ${result.TotalPrice:F2}\n\n" +
                    "Please wait for your number to be called.")
                    .Header("[green]Order Confirmed![/]"));
                break;

            case OrderResultStatus.PaymentDeclined:
                AnsiConsole.MarkupLine("[bold red]Payment didn't go through — would you like to try again?[/]");
                break;

            case OrderResultStatus.GatewayUnavailable:
                AnsiConsole.MarkupLine("[bold yellow]Payment temporarily unavailable. Please pay at the counter.[/]");
                break;

            default:
                AnsiConsole.MarkupLine($"[red]Error: {result.ErrorMessage}[/]");
                break;
        }

        AnsiConsole.MarkupLine("\nPress any key to continue...");
        Console.ReadKey(true);
    }

    private static decimal CalculateDisplayPrice(DrinkDto drink, DrinkCustomisationDto customisation)
    {
        var price = drink.BasePrice;
        if (customisation.Milk == "Oat") price += 0.50m;
        price += (customisation.Toppings?.Count ?? 0) * 0.75m;
        return price;
    }

    private static string[] FormatMilkOptions(string[] milkOptions) =>
        milkOptions.Select(m => m == "Oat" ? "Oat (+$0.50)" : m).ToArray();

    private static string ExtractMilkValue(string display) =>
        display.Contains("(+") ? display.Split(' ')[0] : display;

    private static string[] FormatToppingOptions(string[] toppings) =>
        toppings.Select(t => $"{t} (+$0.75)").ToArray();

    private static string ExtractToppingValue(string display) =>
        display.Split(' ')[0];
}
