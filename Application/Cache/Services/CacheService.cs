using System.Collections.Concurrent;

namespace Application.Cache.Services;

public class CacheService<T>
{
    private readonly ConcurrentDictionary<string, T> _data = [];

    public Task<T?> GetAsync(string key)
    {
        return Task.FromResult(_data.GetValueOrDefault(key));
    }

    public Task<List<T>> GetAllAsync()
    {
        return Task.FromResult(_data.Values.ToList());
    }

    public Task SetAsync(string key, T item)
    {
        _data[key] = item;
        return Task.CompletedTask;
    }
}