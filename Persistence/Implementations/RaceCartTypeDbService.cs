using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Services;

namespace Persistence.Implementations;

/// <inheritdoc />
public class RaceCartTypeDbService : DbService<RaceCartTypeEntity, RaceCartTypeComplex>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RaceCartTypeDbService" /> class
    ///     and configures the specific SQL queries for Race Cart Type entities.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="policy">The policy governing access to race cart type entities.</param>
    public RaceCartTypeDbService(IConfiguration configuration, ModelPolicy<RaceCartTypeEntity> policy)
        : base(configuration, policy)
    {
        QueryIds = @"SELECT Id FROM RaceCartType";
        QuerySimple = @"SELECT * FROM RaceCartType";
        QuerySimpleById = @"SELECT * FROM RaceCartType WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Type FROM RaceCartType";
        QueryComplexById = @"SELECT Id, Type FROM RaceCartType WHERE Id = @Id";
    }

    /// <inheritdoc />
    private protected override async Task<List<RaceCartTypeComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<RaceCartTypeComplex>(QueryComplex);
        return data.ToList();
    }

    /// <inheritdoc />
    private protected override async Task<RaceCartTypeComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<RaceCartTypeComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}