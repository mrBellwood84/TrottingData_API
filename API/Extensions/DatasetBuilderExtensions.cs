using Application.DatasetBuilder.Implementations;
using Application.DatasetBuilder.Interfaces;
using Models.Datasets;

namespace API.Extensions;

public static class DatasetBuilderExtensions
{
    public static IServiceCollection AddDatasetBuilders(this IServiceCollection services)
    {
        services.AddTransient<IDatasetBuilderService<DatasetRaceCard>, DatasetBuildRaceCard>();
        
        return services;
    }
}