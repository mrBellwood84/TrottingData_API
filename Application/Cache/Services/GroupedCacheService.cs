using System.Collections.Concurrent;
using Models.Interfaces;

namespace Application.Cache.Services;

/// <summary>
///     In-memory cache service that groups entities under a parent key.
///     Uses a nested concurrent structure to guarantee thread-safe lookups, updates, and deduplication.
/// </summary>
public class GroupedCacheService<T> where T : IEntity
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, T>> _data = [];

    /// <summary>
    ///     Retrieves the list of grouped items associated with the specified key.
    ///     Returns null if the group key does not exist.
    /// </summary>
    public Task<List<T>?> GetAsync(string key)
    {
        if (_data.TryGetValue(key, out var innerDict))
            // Thread-safe conversion to list even if the collection is actively being modified
            return Task.FromResult<List<T>?>(innerDict.Values.ToList());

        return Task.FromResult<List<T>?>(null);
    }

    /// <summary>
    ///     Stores or updates a collection of items within the specified group key.
    /// </summary>
    public Task SetAsync(string key, List<T> data)
    {
        var innerDict = _data.GetOrAdd(key, _ => []);

        foreach (var item in data) innerDict[item.Id] = item;

        return Task.CompletedTask;
    }
}