using Models.Complex;
using Models.Simple;
using Persistence.Implementations;
using Persistence.Interfaces;

namespace API.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IDbService<DriverLicenseEntity, DriverLicenseComplex>, DriverLicenseDbService>();

        return services;
    }
}