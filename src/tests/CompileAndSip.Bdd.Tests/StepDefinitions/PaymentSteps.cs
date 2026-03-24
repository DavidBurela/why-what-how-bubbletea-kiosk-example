using System.Net.Http.Json;
using CompileAndSip.Bdd.Tests.Support;
using CompileAndSip.OrderApi;
using FluentAssertions;
using Reqnroll;

namespace CompileAndSip.Bdd.Tests.StepDefinitions;

[Binding]
public sealed class PaymentSteps
{
    private readonly TestContext _context;

    public PaymentSteps(TestContext context) => _context = context;

    [Given("the payment will succeed")]
    public void GivenThePaymentWillSucceed()
    {
        // Default behaviour — no configuration needed
    }

    [Given("the payment will be declined")]
    public void GivenThePaymentWillBeDeclined()
    {
        _context.PaymentGateway.ConfigureNextResult(
            new PaymentResult(false, "Card declined"));
    }

    [Given("the payment provider is unavailable")]
    public void GivenThePaymentProviderIsUnavailable()
    {
        _context.PaymentGateway.ConfigureNextResult(
            new PaymentResult(false, "Gateway unavailable", GatewayUnavailable: true));
    }

    [Then("they are told the payment did not go through")]
    public void ThenTheyAreToldThePaymentDidNotGoThrough()
    {
        ((int)_context.LastResponse.StatusCode).Should().Be(402);
    }

    [Then("they are advised to pay at the counter")]
    public void ThenTheyAreAdvisedToPayAtTheCounter()
    {
        ((int)_context.LastResponse.StatusCode).Should().Be(503);
    }

    [Then("no order is sent to the kitchen")]
    public async Task ThenNoOrderIsSentToTheKitchen()
    {
        var response = await _context.Client.GetAsync("/orders/active");
        var active = await response.Content.ReadFromJsonAsync<List<ActiveOrderDto>>();
        active!.Should().BeEmpty();
    }
}
