namespace CompileAndSip.OrderApi;

public record PaymentResult(bool Succeeded, string? ErrorMessage = null, bool GatewayUnavailable = false);

public interface IPaymentGateway
{
    Task<PaymentResult> ProcessPaymentAsync(decimal amount);
}
