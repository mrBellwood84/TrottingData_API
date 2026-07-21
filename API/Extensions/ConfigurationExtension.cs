using Application.Configurations;

namespace API.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddConfigurations(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<DatasetBuilderOptions>(
            configuration.GetSection("DatasetBuilderOptions"));

        return services;
    }
}
