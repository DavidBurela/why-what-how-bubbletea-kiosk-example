using System.Net.Http.Json;
using CompileAndSip.Bdd.Tests.Support;
using CompileAndSip.OrderApi;
using FluentAssertions;
using Reqnroll;

namespace CompileAndSip.Bdd.Tests.StepDefinitions;

[Binding]
public sealed class MenuSteps
{
    private readonly TestContext _context;

    public MenuSteps(TestContext context) => _context = context;

    [Given("the customer is at the kiosk")]
    public void GivenTheCustomerIsAtTheKiosk()
    {
        // No action needed — client is ready
    }

    [When("they view the menu")]
    public async Task WhenTheyViewTheMenu()
    {
        _context.LastResponse = await _context.Client.GetAsync("/menu");
        _context.LastMenuResponse = await _context.LastResponse.Content.ReadFromJsonAsync<MenuResponse>();
    }

    [Then("they see {int} drinks available")]
    public void ThenTheySeeNDrinksAvailable(int count)
    {
        _context.LastMenuResponse!.Drinks.Should().HaveCount(count);
    }

    [Then("{string} is available at ${decimal}")]
    public void ThenDrinkIsAvailableAtPrice(string drinkName, decimal price)
    {
        var drink = _context.LastMenuResponse!.Drinks.Single(d => d.Name == drinkName);
        drink.BasePrice.Should().Be(price);
    }

    [Then("{string} is described as {string}")]
    public void ThenDrinkIsDescribedAs(string drinkName, string description)
    {
        var drink = _context.LastMenuResponse!.Drinks.Single(d => d.Name == drinkName);
        drink.Description.Should().Be(description);
    }
}
