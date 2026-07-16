using Models.Interfaces;

namespace Application.Cache.Interfaces;

public interface ISourceItemCache<T> : ISingleItemCache<T> where T : ISourcedEntity
{
    /// <summary>
    ///     Retrieves a cached item by its external source identifier.
    /// </summary>
    Task<T?> GetSourceAsync(string key);
}