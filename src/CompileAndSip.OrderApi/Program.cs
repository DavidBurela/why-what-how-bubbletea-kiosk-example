using CompileAndSip.OrderApi;

var builder = WebApplication.CreateBuilder(args);

var fakeGateway = new FakePaymentGateway();
builder.Services.AddSingleton<IPaymentGateway>(fakeGateway);
builder.Services.AddSingleton(fakeGateway);
builder.Services.AddSingleton<IOrderStore, InMemoryOrderStore>();

var app = builder.Build();

app.MapGet("/", () => "Compile & Sip Order API");

app.MapGet("/menu", () =>
{
    var drinks = MenuService.GetDrinks().Select(d => new DrinkDto(d.Id, d.Name, d.Description, d.BasePrice)).ToList();
    var defaults = MenuService.GetDefaultCustomisation();
    var options = new CustomisationOptionsDto(
        MilkOptions: Enum.GetNames<MilkType>(),
        ToppingOptions: Enum.GetNames<Topping>(),
        SugarLevels: Enum.GetNames<SugarLevel>(),
        IceLevels: Enum.GetNames<IceLevel>(),
        Temperatures: Enum.GetNames<Temperature>(),
        Defaults: new DrinkCustomisationDto(
            defaults.Milk.ToString(),
            defaults.Toppings.Select(t => t.ToString()).ToList(),
            defaults.Sugar.ToString(),
            defaults.Ice.ToString(),
            defaults.Temperature.ToString()));
    return Results.Ok(new MenuResponse(drinks, options));
});

app.MapPost("/orders", async (OrderRequest request, IPaymentGateway paymentGateway, IOrderStore store) =>
{
    var customisation = MapCustomisation(request.Customisation);
    var errors = OrderValidator.Validate(request.DrinkId, customisation);
    if (errors.Count > 0)
        return Results.BadRequest(new ErrorResponse(string.Join("; ", errors)));

    var drink = MenuService.GetDrinks().First(d => d.Id == request.DrinkId);
    var price = PricingService.CalculatePrice(drink, customisation);

    var paymentResult = await paymentGateway.ProcessPaymentAsync(price);
    if (!paymentResult.Succeeded)
    {
        if (paymentResult.GatewayUnavailable)
            return Results.StatusCode(503);
        return Results.StatusCode(402);
    }

    var orderNumber = store.NextOrderNumber();
    var item = new OrderItem(drink, customisation, price);
    var order = new Order(item, orderNumber);
    order.MarkPaid();
    store.Add(order);

    return Results.Created($"/orders/{order.Id}", new OrderResponse(order.Id, order.OrderNumber, price));
});

app.MapGet("/orders/active", (IOrderStore store) =>
{
    var active = store.GetActiveOrders().Select(o => new ActiveOrderDto(
        o.Id,
        o.OrderNumber,
        o.Item.Drink.Name,
        new DrinkCustomisationDto(
            o.Item.Customisation.Milk.ToString(),
            o.Item.Customisation.Toppings.Select(t => t.ToString()).ToList(),
            o.Item.Customisation.Sugar.ToString(),
            o.Item.Customisation.Ice.ToString(),
            o.Item.Customisation.Temperature.ToString()),
        o.Item.CalculatedPrice,
        o.CreatedAt)).ToList();
    return Results.Ok(active);
});

app.MapPost("/orders/{id}/complete", (Guid id, IOrderStore store) =>
{
    var order = store.GetById(id);
    if (order is null)
        return Results.NotFound(new ErrorResponse("Order not found"));

    order.MarkComplete();
    return Results.Ok();
});

app.Run();

static DrinkCustomisation MapCustomisation(DrinkCustomisationDto? dto)
{
    if (dto is null) return new DrinkCustomisation();

    var milk = dto.Milk is not null && Enum.TryParse<MilkType>(dto.Milk, true, out var m) ? m : MilkType.Regular;
    var toppings = dto.Toppings?.Select(t => Enum.TryParse<Topping>(t, true, out var tp) ? tp : (Topping?)null)
        .Where(t => t.HasValue).Select(t => t!.Value).ToList() ?? [];
    var sugar = dto.Sugar is not null && Enum.TryParse<SugarLevel>(dto.Sugar, true, out var s) ? s : SugarLevel.Hundred;
    var ice = dto.Ice is not null && Enum.TryParse<IceLevel>(dto.Ice, true, out var i) ? i : IceLevel.RegularIce;
    var temp = dto.Temperature is not null && Enum.TryParse<Temperature>(dto.Temperature, true, out var t2) ? t2 : Temperature.Cold;

    return new DrinkCustomisation(milk, toppings, sugar, ice, temp);
}

public partial class Program { } // Enables WebApplicationFactory in tests
