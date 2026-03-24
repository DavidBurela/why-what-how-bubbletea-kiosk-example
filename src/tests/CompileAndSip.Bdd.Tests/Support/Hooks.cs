using CompileAndSip.OrderApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;

namespace CompileAndSip.Bdd.Tests.Support;

[Binding]
public sealed class Hooks
{
    private readonly TestContext _context;
    private WebApplicationFactory<Program>? _factory;

    public Hooks(TestContext context)
    {
        _context = context;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _factory = new WebApplicationFactory<Program>();
        _context.Client = _factory.CreateClient();
        _context.PaymentGateway = _factory.Services.GetRequiredService<FakePaymentGateway>();
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _context.Client?.Dispose();
        _factory?.Dispose();
    }
}
