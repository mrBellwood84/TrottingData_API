using System.Collections.Concurrent;

namespace Application.Cache.Services;

public class CacheService<T>
{
    private readonly ConcurrentDictionary<string, T> _data = [];
    private readonly T? _defaultResult = default;

    public Task<T?> Get(string key)
    {
        var exists = _data.TryGetValue(key, out var value);
        return Task.FromResult(!exists ? _defaultResult : value);
    }

    public Task<List<T>> GetAll()
    {
        return Task.FromResult(_data.Values.ToList());
    }

    public Task Set(string? key, T value)
    {
        _data[key] = value;
        return Task.CompletedTask;
    }
}