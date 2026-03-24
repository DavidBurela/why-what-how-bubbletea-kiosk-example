using System.Net;
using System.Net.Http.Json;
using CompileAndSip.Bdd.Tests.Support;
using CompileAndSip.OrderApi;
using FluentAssertions;
using Reqnroll;

namespace CompileAndSip.Bdd.Tests.StepDefinitions;

[Binding]
public sealed class OrderSteps
{
    private readonly TestContext _context;

    private static readonly Dictionary<string, string> DrinkNameToId = new()
    {
        ["Classic Milk Tea"] = "classic-milk-tea",
        ["Taro Milk Tea"] = "taro-milk-tea",
        ["Mango Green Tea"] = "mango-green-tea"
    };

    public OrderSteps(TestContext context) => _context = context;

    [When("they order a {string} with default options")]
    public async Task WhenTheyOrderWithDefaultOptions(string drinkName)
    {
        var request = new OrderRequest(DrinkNameToId[drinkName]);
        _context.LastResponse = await _context.Client.PostAsJsonAsync("/orders", request);
        if (_context.LastResponse.IsSuccessStatusCode)
            _context.LastOrderResponse = await _context.LastResponse.Content.ReadFromJsonAsync<OrderResponse>();
    }

    [When("they order a {string} with oat milk")]
    public async Task WhenTheyOrderWithOatMilk(string drinkName)
    {
        var request = new OrderRequest(DrinkNameToId[drinkName],
            new DrinkCustomisationDto(Milk: "Oat"));
        _context.LastResponse = await _context.Client.PostAsJsonAsync("/orders", request);
        if (_context.LastResponse.IsSuccessStatusCode)
            _context.LastOrderResponse = await _context.LastResponse.Content.ReadFromJsonAsync<OrderResponse>();
    }

    [When("they order a {string} with tapioca pearls")]
    public async Task WhenTheyOrderWithTapiocaPearls(string drinkName)
    {
        var request = new OrderRequest(DrinkNameToId[drinkName],
            new DrinkCustomisationDto(Toppings: ["TapiocaPearls"]));
        _context.LastResponse = await _context.Client.PostAsJsonAsync("/orders", request);
        if (_context.LastResponse.IsSuccessStatusCode)
            _context.LastOrderResponse = await _context.LastResponse.Content.ReadFromJsonAsync<OrderResponse>();
    }

    [When("they order a {string} with oat milk, tapioca pearls and coconut jelly")]
    public async Task WhenTheyOrderWithFullCustomisation(string drinkName)
    {
        var request = new OrderRequest(DrinkNameToId[drinkName],
            new DrinkCustomisationDto(Milk: "Oat", Toppings: ["TapiocaPearls", "CoconutJelly"]));
        _context.LastResponse = await _context.Client.PostAsJsonAsync("/orders", request);
        if (_context.LastResponse.IsSuccessStatusCode)
            _context.LastOrderResponse = await _context.LastResponse.Content.ReadFromJsonAsync<OrderResponse>();
    }

    [When("they try to order a {string} with three toppings")]
    public async Task WhenTheyTryToOrderWithThreeToppings(string drinkName)
    {
        var request = new OrderRequest(DrinkNameToId[drinkName],
            new DrinkCustomisationDto(Toppings: ["TapiocaPearls", "CoconutJelly", "Pudding"]));
        _context.LastResponse = await _context.Client.PostAsJsonAsync("/orders", request);
    }

    [Then("the total price is ${decimal}")]
    public void ThenTheTotalPriceIs(decimal expectedPrice)
    {
        _context.LastOrderResponse!.TotalPrice.Should().Be(expectedPrice);
    }

    [Then("they receive a confirmation with an order number")]
    public void ThenTheyReceiveAConfirmationWithAnOrderNumber()
    {
        _context.LastResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        _context.LastOrderResponse!.OrderNumber.Should().BeGreaterThan(0);
    }

    [Then("the order is rejected")]
    public void ThenTheOrderIsRejected()
    {
        _context.LastResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Then("the order appears on the kitchen display")]
    public async Task ThenTheOrderAppearsOnTheKitchenDisplay()
    {
        var response = await _context.Client.GetAsync("/orders/active");
        var active = await response.Content.ReadFromJsonAsync<List<ActiveOrderDto>>();
        active!.Should().Contain(o => o.Id == _context.LastOrderResponse!.Id);
    }
}
