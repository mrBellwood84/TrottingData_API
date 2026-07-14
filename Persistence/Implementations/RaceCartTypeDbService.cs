using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
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
    public RaceCartTypeDbService(IConfiguration configuration)
        : base(configuration)
    {
        QueryIds = @"SELECT Id FROM RaceCartType";
        QueryEntity = @"SELECT * FROM RaceCartType";
        QueryEntityById = @"SELECT * FROM RaceCartType WHERE Id = @Id";
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