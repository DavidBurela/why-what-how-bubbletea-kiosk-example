using System.Net.Http.Json;

namespace CompileAndSip.KitchenDisplay;

public interface IOrderApiClient
{
    Task<List<ActiveOrderDto>> GetActiveOrdersAsync();
    Task<bool> MarkOrderCompleteAsync(Guid orderId);
}

public sealed class OrderApiClient : IOrderApiClient
{
    private readonly HttpClient _httpClient;

    public OrderApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ActiveOrderDto>> GetActiveOrdersAsync()
    {
        var response = await _httpClient.GetAsync("/orders/active");
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<ActiveOrderDto>>())!;
    }

    public async Task<bool> MarkOrderCompleteAsync(Guid orderId)
    {
        var response = await _httpClient.PostAsync($"/orders/{orderId}/complete", null);
        return response.IsSuccessStatusCode;
    }
}
