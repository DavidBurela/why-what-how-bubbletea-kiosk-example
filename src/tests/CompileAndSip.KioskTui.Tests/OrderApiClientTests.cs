using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CompileAndSip.KioskTui;
using FluentAssertions;

namespace CompileAndSip.KioskTui.Tests;

public sealed class OrderApiClientTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private static HttpClient CreateMockClient(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        var mockHandler = new MockHttpMessageHandler(handler);
        return new HttpClient(mockHandler) { BaseAddress = new Uri("http://localhost:5100") };
    }

    [Fact]
    public async Task GetMenuAsync_ReturnsMenu()
    {
        var menu = new MenuResponse(
            [
                new DrinkDto("classic-milk-tea", "Classic Milk Tea", "Test", 5.50m),
                new DrinkDto("taro-milk-tea", "Taro Milk Tea", "Test", 6.00m),
                new DrinkDto("mango-green-tea", "Mango Green Tea", "Test", 6.00m)
            ],
            new CustomisationOptionsDto(
                ["Regular", "Oat", "None"],
                ["TapiocaPearls", "CoconutJelly", "Pudding"],
                ["Zero", "TwentyFive", "Fifty", "SeventyFive", "Hundred"],
                ["NoIce", "LessIce", "RegularIce"],
                ["Hot", "Cold"],
                new DrinkCustomisationDto("Regular", [], "Hundred", "RegularIce", "Cold")));

        var client = CreateMockClient(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(menu, options: JsonOptions)
            });

        var apiClient = new OrderApiClient(client);
        var result = await apiClient.GetMenuAsync();
        result.Drinks.Should().HaveCount(3);
    }

    [Fact]
    public async Task PlaceOrderAsync_Success_ReturnsOrderNumber()
    {
        var orderResponse = new OrderResponse(Guid.NewGuid(), 1, 5.50m);
        var client = CreateMockClient(_ =>
            new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = JsonContent.Create(orderResponse, options: JsonOptions)
            });

        var apiClient = new OrderApiClient(client);
        var result = await apiClient.PlaceOrderAsync(new OrderRequest("classic-milk-tea"));
        result.Status.Should().Be(OrderResultStatus.Success);
        result.OrderNumber.Should().Be(1);
        result.TotalPrice.Should().Be(5.50m);
    }

    [Fact]
    public async Task PlaceOrderAsync_Declined_ReturnsError()
    {
        var client = CreateMockClient(_ =>
            new HttpResponseMessage(HttpStatusCode.PaymentRequired));

        var apiClient = new OrderApiClient(client);
        var result = await apiClient.PlaceOrderAsync(new OrderRequest("classic-milk-tea"));
        result.Status.Should().Be(OrderResultStatus.PaymentDeclined);
    }

    [Fact]
    public async Task PlaceOrderAsync_Unavailable_ReturnsUnavailable()
    {
        var client = CreateMockClient(_ =>
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));

        var apiClient = new OrderApiClient(client);
        var result = await apiClient.PlaceOrderAsync(new OrderRequest("classic-milk-tea"));
        result.Status.Should().Be(OrderResultStatus.GatewayUnavailable);
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
