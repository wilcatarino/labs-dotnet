var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Say hello to Minimal API! :)");

app.Run();
