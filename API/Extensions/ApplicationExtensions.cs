using Application.Preloader.Service;

namespace API.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPreloaderService, PreloaderService>();
        return services;
    }
    
}