using CompileAndSip.KioskTui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var baseUrl = builder.Configuration.GetValue<string>("OrderApi:BaseUrl") ?? "http://localhost:5100";

builder.Services.AddHttpClient<IOrderApiClient, OrderApiClient>(client =>
{
    client.BaseAddress = new Uri(baseUrl);
});

var host = builder.Build();
var apiClient = host.Services.GetRequiredService<IOrderApiClient>();
var app = new KioskApp(apiClient);
await app.RunAsync();
