using Spectre.Console;

namespace CompileAndSip.KitchenDisplay;

public sealed class KitchenDisplayApp
{
    private readonly IOrderApiClient _apiClient;
    private readonly int _pollIntervalMs;

    public KitchenDisplayApp(IOrderApiClient apiClient, int pollIntervalMs = 3000)
    {
        _apiClient = apiClient;
        _pollIntervalMs = pollIntervalMs;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("[bold blue]Kitchen Display — Compile & Sip[/]\n");

            List<ActiveOrderDto> orders;
            try
            {
                orders = await _apiClient.GetActiveOrdersAsync();
            }
            catch
            {
                AnsiConsole.MarkupLine("[red]Unable to reach Order API — retrying...[/]");
                await Task.Delay(_pollIntervalMs, cancellationToken);
                continue;
            }

            if (orders.Count == 0)
            {
                AnsiConsole.MarkupLine("[dim]No active orders — waiting for new orders...[/]");
            }
            else
            {
                RenderOrderTable(orders);
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[dim]Press [bold]1-9[/] to complete an order, or wait for refresh...[/]");
            }

            // Check for keypress during poll interval
            var endTime = DateTime.UtcNow.AddMilliseconds(_pollIntervalMs);
            while (DateTime.UtcNow < endTime && !cancellationToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (char.IsDigit(key.KeyChar) && orders.Count > 0)
                    {
                        var index = key.KeyChar - '1';
                        if (index >= 0 && index < orders.Count)
                        {
                            var order = orders[index];
                            var success = await _apiClient.MarkOrderCompleteAsync(order.Id);
                            if (success)
                            {
                                AnsiConsole.MarkupLine($"\n[bold green]Order #{order.OrderNumber} marked complete![/]");
                                await Task.Delay(1000, cancellationToken);
                            }
                            break;
                        }
                    }
                }
                await Task.Delay(100, cancellationToken);
            }
        }
    }

    private static void RenderOrderTable(List<ActiveOrderDto> orders)
    {
        var table = new Table();
        table.AddColumn("#");
        table.AddColumn("Order");
        table.AddColumn("Drink");
        table.AddColumn("Milk");
        table.AddColumn("Toppings");
        table.AddColumn("Sugar");
        table.AddColumn("Ice");
        table.AddColumn("Temp");
        table.AddColumn("Price");

        for (var i = 0; i < orders.Count; i++)
        {
            var o = orders[i];
            var c = o.Customisation;
            table.AddRow(
                (i + 1).ToString(),
                $"#{o.OrderNumber}",
                o.DrinkName,
                c.Milk ?? "Regular",
                c.Toppings?.Count > 0 ? string.Join(", ", c.Toppings) : "None",
                c.Sugar ?? "Hundred",
                c.Ice ?? "RegularIce",
                c.Temperature ?? "Cold",
                $"${o.TotalPrice:F2}");
        }

        AnsiConsole.Write(table);
    }
}
