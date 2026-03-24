using System.Net.Http.Json;
using CompileAndSip.Bdd.Tests.Support;
using CompileAndSip.OrderApi;
using FluentAssertions;
using Reqnroll;

namespace CompileAndSip.Bdd.Tests.StepDefinitions;

[Binding]
public sealed class KitchenSteps
{
    private readonly TestContext _context;

    public KitchenSteps(TestContext context) => _context = context;

    [Then("the kitchen display shows the order with drink {string} at ${decimal}")]
    public async Task ThenTheKitchenDisplayShowsTheOrderWithDrinkAtPrice(string drinkName, decimal price)
    {
        var response = await _context.Client.GetAsync("/orders/active");
        var active = await response.Content.ReadFromJsonAsync<List<ActiveOrderDto>>();
        var order = active!.Single(o => o.Id == _context.LastOrderResponse!.Id);
        order.DrinkName.Should().Be(drinkName);
        order.TotalPrice.Should().Be(price);
    }

    [When("the kitchen staff marks the order as complete")]
    public async Task WhenTheKitchenStaffMarksTheOrderAsComplete()
    {
        _context.LastResponse = await _context.Client.PostAsync(
            $"/orders/{_context.LastOrderResponse!.Id}/complete", null);
    }

    [Then("the order is no longer on the active list")]
    public async Task ThenTheOrderIsNoLongerOnTheActiveList()
    {
        var response = await _context.Client.GetAsync("/orders/active");
        var active = await response.Content.ReadFromJsonAsync<List<ActiveOrderDto>>();
        active!.Should().NotContain(o => o.Id == _context.LastOrderResponse!.Id);
    }
}
