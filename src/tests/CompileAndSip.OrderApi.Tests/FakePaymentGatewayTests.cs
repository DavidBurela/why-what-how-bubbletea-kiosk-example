using CompileAndSip.OrderApi;
using FluentAssertions;

namespace CompileAndSip.OrderApi.Tests;

public sealed class FakePaymentGatewayTests
{
    [Fact]
    public async Task DefaultBehaviour_Succeeds()
    {
        var gateway = new FakePaymentGateway();
        var result = await gateway.ProcessPaymentAsync(5.50m);
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task ConfiguredDecline_Fails()
    {
        var gateway = new FakePaymentGateway();
        gateway.ConfigureNextResult(new PaymentResult(false, "Card declined"));
        var result = await gateway.ProcessPaymentAsync(5.50m);
        result.Succeeded.Should().BeFalse();
        result.ErrorMessage.Should().Be("Card declined");
    }

    [Fact]
    public async Task ConfiguredUnavailable_Fails()
    {
        var gateway = new FakePaymentGateway();
        gateway.ConfigureNextResult(new PaymentResult(false, "Gateway unavailable", GatewayUnavailable: true));
        var result = await gateway.ProcessPaymentAsync(5.50m);
        result.Succeeded.Should().BeFalse();
        result.GatewayUnavailable.Should().BeTrue();
    }
}
