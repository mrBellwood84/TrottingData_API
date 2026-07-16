using Models.Interfaces;

namespace Application.Cache.Interfaces;

public interface IListItemCache<T> : ISingleItemCache<T> where T : IEntity
{
    /// <summary>
    ///     Gets a value indicating whether the full dataset has been loaded into the cache.
    /// </summary>
    bool Loaded { get; }

    /// <summary>
    ///     Retrieves all items currently stored within this cache instance.
    /// </summary>
    Task<List<T>> GetAllAsync();

    /// <summary>
    ///     Populates the cache with a list of items sequentially and marks the collection as fully loaded.
    /// </summary>
    Task SetAsync(List<T> items);
}