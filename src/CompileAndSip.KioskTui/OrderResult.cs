namespace CompileAndSip.KioskTui;

public enum OrderResultStatus
{
    Success,
    PaymentDeclined,
    GatewayUnavailable,
    ValidationError
}

public record OrderResult(
    OrderResultStatus Status,
    int? OrderNumber = null,
    decimal? TotalPrice = null,
    string? ErrorMessage = null);
