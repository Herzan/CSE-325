var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

// Simple root message so hitting the base URL in a browser confirms the API is running
app.MapGet("/", () => "Pizza API is running. Try GET /Pizzas");

app.Run();
