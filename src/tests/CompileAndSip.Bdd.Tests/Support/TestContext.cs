using System.Net.Http.Json;
using CompileAndSip.OrderApi;

namespace CompileAndSip.Bdd.Tests.Support;

public sealed class TestContext
{
    public HttpClient Client { get; set; } = null!;
    public FakePaymentGateway PaymentGateway { get; set; } = null!;
    public HttpResponseMessage LastResponse { get; set; } = null!;
    public MenuResponse? LastMenuResponse { get; set; }
    public OrderResponse? LastOrderResponse { get; set; }
    public List<ActiveOrderDto>? LastActiveOrders { get; set; }
    public string? LastErrorMessage { get; set; }
}
