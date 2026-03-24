using System.Net.Http.Json;

namespace CompileAndSip.KioskTui;

public interface IOrderApiClient
{
    Task<MenuResponse> GetMenuAsync();
    Task<OrderResult> PlaceOrderAsync(OrderRequest request);
}

public sealed class OrderApiClient : IOrderApiClient
{
    private readonly HttpClient _httpClient;

    public OrderApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MenuResponse> GetMenuAsync()
    {
        var response = await _httpClient.GetAsync("/menu");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<MenuResponse>())!;
    }

    public async Task<OrderResult> PlaceOrderAsync(OrderRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/orders", request);

        if (response.IsSuccessStatusCode)
        {
            var orderResponse = await response.Content.ReadFromJsonAsync<OrderResponse>();
            return new OrderResult(OrderResultStatus.Success, orderResponse!.OrderNumber, orderResponse.TotalPrice);
        }

        var statusCode = (int)response.StatusCode;

        if (statusCode == 402)
            return new OrderResult(OrderResultStatus.PaymentDeclined, ErrorMessage: "Payment didn't go through");

        if (statusCode == 503)
            return new OrderResult(OrderResultStatus.GatewayUnavailable, ErrorMessage: "Payment temporarily unavailable");

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        return new OrderResult(OrderResultStatus.ValidationError, ErrorMessage: error?.Error ?? "Unknown error");
    }
}
