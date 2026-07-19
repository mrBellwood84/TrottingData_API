using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Register all application services (Dependency Injection container)
builder.Services
    .AddApiServices()
    .AddCache()
    .AddDatasetBuilders()
    .AddPersistence()
    .AddRepositoryServices();

// 2. Enable strict dependency injection validation (Fail-Fast on startup)
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

var app = builder.Build();

// 3. Configure the HTTP request pipeline (Middleware)
app.UseApiPipeline();

app.Run();