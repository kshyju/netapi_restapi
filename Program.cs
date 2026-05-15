using Serilog;

var port = Environment.GetEnvironmentVariable("FUNCTIONS_CUSTOMHANDLER_PORT") ?? "5080";
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://localhost:{port}");
builder.Host.UseSerilog((context, config) => config
    .WriteTo.Console()
    .WriteTo.File("logs/requests.log", rollingInterval: RollingInterval.Day));

var app = builder.Build();

app.UseSerilogRequestLogging();

app.Logger.LogInformation("Listening on port {Port}", port);

var products = new List<Product>
{
    new(1, "Laptop", 999.99m),
    new(2, "Mouse", 29.99m),
    new(3, "Keyboard", 59.99m),
    new(4, "Monitor", 349.99m),
    new(5, "Headset", 79.99m)
};

app.MapGet("/products", () => products);

app.MapGet("/api/items", () => products);

app.MapGet("/products/{id:int}", (int id) =>
    products.FirstOrDefault(p => p.Id == id) is { } product
        ? Results.Ok(product)
        : Results.NotFound());

app.Run();

record Product(int Id, string Name, decimal Price);
