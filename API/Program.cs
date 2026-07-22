using API.Extensions;
using Application.Preloader.Service;

var builder = WebApplication.CreateBuilder(args);

// 1. Register all application services (Dependency Injection container)
builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddCache()
    .AddConfigurations(builder.Configuration)
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


// 3. Preload data if settings says yes
var runPreloading = app.Configuration.GetValue<bool>("RunPreloading");
if (runPreloading)
{
    Console.WriteLine("PRELOADING DATA...");
    using var scope = app.Services.CreateScope();
    var preloader = scope.ServiceProvider.GetRequiredService<IPreloaderService>();
    await preloader.PreloadAllAsync();
    Console.WriteLine("PRELOAD DATA DONE!");
}

// 4. Configure the HTTP request pipeline (Middleware)
app.UseApiPipeline();

app.Run();