using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Simple;

namespace API.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryService<DriverLicenseEntity, DriverLicenseComplex>, DriverLicenseRepository>();
        
        return services;
    }
}