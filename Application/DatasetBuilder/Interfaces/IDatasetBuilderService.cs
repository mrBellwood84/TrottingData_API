using Models.Datasets;

namespace Application.DatasetBuilder.Interfaces;

public interface IDatasetBuilderService<T>
{
    Task<T> BuildAsync(string raceId);
}