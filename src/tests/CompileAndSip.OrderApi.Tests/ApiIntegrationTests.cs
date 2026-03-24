using System.Net;
using System.Net.Http.Json;
using CompileAndSip.OrderApi;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CompileAndSip.OrderApi.Tests;

public sealed class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient() => _factory.CreateClient();

    private FakePaymentGateway GetGateway() =>
        _factory.Services.GetRequiredService<FakePaymentGateway>();

    private static OrderRequest ValidOrder(DrinkCustomisationDto? customisation = null) =>
        new("classic-milk-tea", customisation);

    [Fact]
    public async Task GetMenu_Returns200WithThreeDrinks()
    {
        var client = CreateClient();
        var response = await client.GetAsync("/menu");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var menu = await response.Content.ReadFromJsonAsync<MenuResponse>();
        menu!.Drinks.Should().HaveCount(3);
    }

    [Fact]
    public async Task PostOrder_ValidOrder_Returns201()
    {
        using var scope = _factory.Services.CreateScope();
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/orders", ValidOrder());
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var order = await response.Content.ReadFromJsonAsync<OrderResponse>();
        order!.OrderNumber.Should().BeGreaterThan(0);
        order.TotalPrice.Should().Be(5.50m);
    }

    [Fact]
    public async Task PostOrder_ValidOrder_AppearsInActive()
    {
        var client = CreateClient();
        var postResponse = await client.PostAsJsonAsync("/orders", ValidOrder());
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var orderResponse = await postResponse.Content.ReadFromJsonAsync<OrderResponse>();

        var activeResponse = await client.GetAsync("/orders/active");
        var active = await activeResponse.Content.ReadFromJsonAsync<List<ActiveOrderDto>>();
        active!.Should().Contain(o => o.Id == orderResponse!.Id);
    }

    [Fact]
    public async Task PostOrder_SequentialNumbers()
    {
        var client = CreateClient();
        var r1 = await client.PostAsJsonAsync("/orders", ValidOrder());
        var r2 = await client.PostAsJsonAsync("/orders", ValidOrder());
        var o1 = await r1.Content.ReadFromJsonAsync<OrderResponse>();
        var o2 = await r2.Content.ReadFromJsonAsync<OrderResponse>();
        o2!.OrderNumber.Should().Be(o1!.OrderNumber + 1);
    }

    [Fact]
    public async Task PostOrder_PaymentDeclined_Returns402()
    {
        var client = CreateClient();
        GetGateway().ConfigureNextResult(new PaymentResult(false, "Card declined"));
        var response = await client.PostAsJsonAsync("/orders", ValidOrder());
        response.StatusCode.Should().Be(HttpStatusCode.PaymentRequired);
    }

    [Fact]
    public async Task PostOrder_PaymentDeclined_NotStored()
    {
        var client = CreateClient();
        var activeBefore = await client.GetFromJsonAsync<List<ActiveOrderDto>>("/orders/active");
        var countBefore = activeBefore!.Count;

        GetGateway().ConfigureNextResult(new PaymentResult(false, "Card declined"));
        await client.PostAsJsonAsync("/orders", ValidOrder());

        var activeAfter = await client.GetFromJsonAsync<List<ActiveOrderDto>>("/orders/active");
        activeAfter!.Count.Should().Be(countBefore);
    }

    [Fact]
    public async Task PostOrder_GatewayUnavailable_Returns503()
    {
        var client = CreateClient();
        GetGateway().ConfigureNextResult(new PaymentResult(false, "Unavailable", GatewayUnavailable: true));
        var response = await client.PostAsJsonAsync("/orders", ValidOrder());
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task PostOrder_GatewayUnavailable_NotStored()
    {
        var client = CreateClient();
        var activeBefore = await client.GetFromJsonAsync<List<ActiveOrderDto>>("/orders/active");
        var countBefore = activeBefore!.Count;

        GetGateway().ConfigureNextResult(new PaymentResult(false, "Unavailable", GatewayUnavailable: true));
        await client.PostAsJsonAsync("/orders", ValidOrder());

        var activeAfter = await client.GetFromJsonAsync<List<ActiveOrderDto>>("/orders/active");
        activeAfter!.Count.Should().Be(countBefore);
    }

    [Fact]
    public async Task PostOrder_InvalidDrink_Returns400()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/orders", new OrderRequest("unknown-drink"));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostOrder_TooManyToppings_Returns400()
    {
        var client = CreateClient();
        var customisation = new DrinkCustomisationDto(
            Toppings: ["TapiocaPearls", "CoconutJelly", "Pudding"]);
        var response = await client.PostAsJsonAsync("/orders", new OrderRequest("classic-milk-tea", customisation));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetActiveOrders_Empty_Returns200()
    {
        // Use a fresh factory to start with no orders
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.GetAsync("/orders/active");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var active = await response.Content.ReadFromJsonAsync<List<ActiveOrderDto>>();
        active!.Should().BeEmpty();
    }

    [Fact]
    public async Task PostComplete_PaidOrder_Returns200()
    {
        var client = CreateClient();
        var postResponse = await client.PostAsJsonAsync("/orders", ValidOrder());
        var order = await postResponse.Content.ReadFromJsonAsync<OrderResponse>();

        var completeResponse = await client.PostAsync($"/orders/{order!.Id}/complete", null);
        completeResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var active = await client.GetFromJsonAsync<List<ActiveOrderDto>>("/orders/active");
        active!.Should().NotContain(o => o.Id == order.Id);
    }

    [Fact]
    public async Task PostComplete_NotFound_Returns404()
    {
        var client = CreateClient();
        var response = await client.PostAsync($"/orders/{Guid.NewGuid()}/complete", null);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
