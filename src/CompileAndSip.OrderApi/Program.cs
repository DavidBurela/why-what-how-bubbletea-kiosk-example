var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Compile & Sip Order API");

app.Run();

public partial class Program { } // Enables WebApplicationFactory in tests
