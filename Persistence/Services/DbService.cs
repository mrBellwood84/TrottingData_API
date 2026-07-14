using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using Persistence.Exceptions;
using Persistence.Interfaces;

namespace Persistence.Services;

public class DbService<TSimple, TComplex>(IConfiguration configuration, ModelPolicy<TSimple> policy)
    : DbConnection(configuration), IDbService<TSimple, TComplex>
{
    internal string QueryIds { get; init; } = string.Empty;
    internal string QuerySimple { get; init; } = string.Empty;
    internal string QuerySimpleById { get; init; } = string.Empty;
    internal string QueryComplex { get; init; } = string.Empty;
    internal string QueryComplexById { get; init; } = string.Empty;

    public async Task<List<IdModel>> GetIdsAsync()
    {
        if (string.IsNullOrEmpty(QueryIds))
            throw new PersistenceMissingQueryException($"Missing QueryIds for {typeof(TComplex).Name})");

        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<IdModel>(QueryIds);
        return data.ToList();
    }

    public async Task<List<TSimple>> GetAllSimpleAsync()
    {
        if (!policy.AllowAllSimple)
            throw new PersistenceMissingQueryException($"GetAllSimple for {typeof(TSimple).Name}) is disallowed");
        if (string.IsNullOrEmpty(QuerySimple))
            throw new PersistenceMissingQueryException($"Missing QuerySimple for {typeof(TSimple).Name})");

        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<TSimple>(QuerySimple);
        return data.ToList();
    }

    public async Task<TSimple?> GetSimpleByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(QuerySimpleById))
            throw new PersistenceMissingQueryException($"Missing QuerySimpleById for {typeof(TSimple).Name})");

        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<TSimple>(QuerySimpleById, new { Id = id });
        return data;
    }

    public async Task<List<TComplex>> GetAllComplexAsync()
    {
        if (!policy.AllowAllComplex)
            throw new PersistenceQueryNotAllowedException($"GetAllComplex for {typeof(TComplex).Name} is disallowed");
        if (string.IsNullOrEmpty(QueryComplex))
            throw new PersistenceMissingQueryException($"Missing QueryComplex for {typeof(TComplex).Name})");

        var data = await GetAllComplexLogicAsync();
        return data;
    }

    public async Task<TComplex?> GetComplexByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(QueryComplexById))
            throw new PersistenceMissingQueryException($"Missing QueryComplexById for {typeof(TComplex).Name})");

        var data = GetComplexByIdLogicAsync(id);
        return await data;
    }

    private protected virtual Task<List<TComplex>> GetAllComplexLogicAsync()
    {
        throw new PersistenceNotImplementedException(
            $"GetAllComplex logic for {typeof(TComplex).Name} is not implemented");
    }

    private protected virtual Task<TComplex?> GetComplexByIdLogicAsync(string id)
    {
        throw new PersistenceNotImplementedException(
            $"GetComplexByIdAsync logic for {typeof(TComplex).Name} is not implemented");
    }
}