using Microsoft.EntityFrameworkCore;
using reliefo_api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// API test endpoint
app.MapGet("/api/", () => Results.Text("Reliefo API"));

// version endpoint
var version = "1";
var releaseDate = "xx.05.2026";
app.MapGet("/api/version", () => Results.Json(new { version, releaseDate }));

app.UseDefaultFiles(); // rewrites / to /index.html
app.MapStaticAssets(); // serves wwwroot with fingerprinting + compression

app.MapControllers();

app.MapFallbackToFile("index.html"); // Angular client-side routing

app.Run();
