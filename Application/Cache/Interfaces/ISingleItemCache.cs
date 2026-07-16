using Models.Interfaces;

namespace Application.Cache.Interfaces;

public interface ISingleItemCache<T> where T : IEntity
{
    /// <summary>
    ///     Retrieves a cached item by its unique identifier.
    /// </summary>
    Task<T?> GetAsync(string key);

    /// <summary>
    ///     Caches a single item using its primary entity identifier as the key.
    /// </summary>
    Task SetAsync(T item);
}