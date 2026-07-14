using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Entity;

namespace API.Extensions;

/// <summary>
///     Extension methods for configuring and registering repository services.
/// </summary>
public static class RepositoryExtensions
{
    /// <summary>
    ///     Registers all repository services (IRepositoryService) in the Dependency Injection container.
    /// </summary>
    /// <param name="services">The service collection to add the repository services to.</param>
    /// <returns>The updated <see cref="IServiceCollection" />.</returns>
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        // the flat ones ones <3
        services.AddScoped<IRepositoryService<DriverLicenseEntity, DriverLicenseComplex>, DriverLicenseRepository>();
        services.AddScoped<IRepositoryService<HorseSexEntity, HorseSexComplex>, HorseSexRepository>();
        services.AddScoped<IRepositoryService<HorseTypeEntity, HorseTypeComplex>, HorseTypeRepository>();
        services.AddScoped<IRepositoryService<RaceCartTypeEntity, RaceCartTypeComplex>, RaceCartTypeRepository>();
        services.AddScoped<IRepositoryService<RaceCourseEntity, RaceCourseComplex>, RaceCourseRepository>();
        services
            .AddScoped<IRepositoryService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>,
                RaceGamblingTypeRepository>();
        services.AddScoped<IRepositoryService<RaceStartTypeEntity, RaceStartTypeComplex>, RaceStartTypeRepository>();
        
        // the sourced ones ^^
        services.AddScoped<ISourcedRepositoryService<DriverEntity, DriverComplex>, DriverRepository>();
        services.AddScoped<ISourcedRepositoryService<HorseEntity, HorseComplex>, HorseRepository>();

        return services;
    }
}