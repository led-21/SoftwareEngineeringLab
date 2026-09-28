using SoftwareEngineeringLab.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS for frontend Vite development server (localhost:5173) and any origin for local lab demos
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Root information endpoint
app.MapGet("/", () => Results.Ok(new
{
    name = "Software Engineering Lab API",
    status = "healthy",
    version = "1.0.0",
    docs = new[]
    {
        "/api/algorithms/binary-search",
        "/api/algorithms/sort",
        "/api/algorithms/islands",
        "/api/system-design/rate-limiter/status",
        "/api/system-design/url-shortener/shorten",
        "/api/system-design/lru-cache",
        "/api/system-design/load-balancer/next",
        "/api/system-design/consistent-hashing/route",
        "/api/solid",
        "/api/design-patterns"
    }
}));

// Map all modular endpoints
app.MapAlgorithmEndpoints();
app.MapSystemDesignEndpoints();
app.MapSolidEndpoints();
app.MapDesignPatternEndpoints();

app.Run();
