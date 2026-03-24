using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CompileAndSip.KitchenDisplay;
using FluentAssertions;

namespace CompileAndSip.KitchenDisplay.Tests;

public sealed class OrderApiClientTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private static HttpClient CreateMockClient(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        var mockHandler = new MockHttpMessageHandler(handler);
        return new HttpClient(mockHandler) { BaseAddress = new Uri("http://localhost:5100") };
    }

    [Fact]
    public async Task GetActiveOrdersAsync_ReturnsOrders()
    {
        var orders = new List<ActiveOrderDto>
        {
            new(Guid.NewGuid(), 1, "Classic Milk Tea",
                new DrinkCustomisationDto("Regular", [], "Hundred", "RegularIce", "Cold"),
                5.50m, DateTimeOffset.UtcNow)
        };

        var client = CreateMockClient(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(orders, options: JsonOptions)
            });

        var apiClient = new OrderApiClient(client);
        var result = await apiClient.GetActiveOrdersAsync();
        result.Should().HaveCount(1);
        result[0].DrinkName.Should().Be("Classic Milk Tea");
    }

    [Fact]
    public async Task GetActiveOrdersAsync_Empty_ReturnsEmptyList()
    {
        var client = CreateMockClient(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new List<ActiveOrderDto>(), options: JsonOptions)
            });

        var apiClient = new OrderApiClient(client);
        var result = await apiClient.GetActiveOrdersAsync();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task MarkOrderCompleteAsync_Success_ReturnsTrue()
    {
        var client = CreateMockClient(_ =>
            new HttpResponseMessage(HttpStatusCode.OK));

        var apiClient = new OrderApiClient(client);
        var result = await apiClient.MarkOrderCompleteAsync(Guid.NewGuid());
        result.Should().BeTrue();
    }

    [Fact]
    public async Task MarkOrderCompleteAsync_NotFound_ReturnsFalse()
    {
        var client = CreateMockClient(_ =>
            new HttpResponseMessage(HttpStatusCode.NotFound));

        var apiClient = new OrderApiClient(client);
        var result = await apiClient.MarkOrderCompleteAsync(Guid.NewGuid());
        result.Should().BeFalse();
    }
}

internal sealed class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

    public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        _handler = handler;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_handler(request));
    }
}
