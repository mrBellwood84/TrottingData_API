using Application.Repository.Implementations;
using Application.Repository.Interfaces;
using Models.Complex;
using Models.Simple;

namespace API.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        // the simple ones <3
        services.AddScoped<IRepositoryService<DriverLicenseEntity, DriverLicenseComplex>, DriverLicenseRepository>();
        services.AddScoped<IRepositoryService<HorseSexEntity, HorseSexComplex>, HorseSexRepository>();
        services.AddScoped<IRepositoryService<HorseTypeEntity, HorseTypeComplex>, HorseTypeRepository>();
        services.AddScoped<IRepositoryService<RaceCartTypeEntity, RaceCartTypeComplex>, RaceCartTypeRepository>();
        services.AddScoped<IRepositoryService<RaceCourseEntity, RaceCourseComplex>, RaceCourseRepository>();
        services
            .AddScoped<IRepositoryService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>,
                RaceGamblingTypeRepository>();
        services.AddScoped<IRepositoryService<RaceStartTypeEntity, RaceStartTypeComplex>, RaceStartTypeRepository>();

        return services;
    }
}