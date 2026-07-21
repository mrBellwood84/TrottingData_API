namespace Application.DatasetBuilder.Interfaces;

public interface IDatasetBuilderService<T>
{
    Task<List<T>> BuildAsync(string raceId);
}