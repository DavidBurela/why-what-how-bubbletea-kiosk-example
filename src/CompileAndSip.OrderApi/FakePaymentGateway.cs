namespace CompileAndSip.OrderApi;

public sealed class FakePaymentGateway : IPaymentGateway
{
    private PaymentResult? _nextResult;

    public void ConfigureNextResult(PaymentResult result)
    {
        _nextResult = result;
    }

    public Task<PaymentResult> ProcessPaymentAsync(decimal amount)
    {
        if (_nextResult is { } result)
        {
            _nextResult = null;
            return Task.FromResult(result);
        }

        return Task.FromResult(new PaymentResult(true));
    }
}
