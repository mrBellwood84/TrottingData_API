using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <inheritdoc />
public class RaceGamblingTypeDbService : DbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RaceGamblingTypeDbService" /> class
    ///     and configures the specific SQL queries for Race Gambling Type entities.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public RaceGamblingTypeDbService(IConfiguration configuration)
        : base(configuration)
    {
        QueryIds = @"SELECT Id FROM RaceGamblingType";
        QueryEntity = @"SELECT * FROM RaceGamblingType";
        QueryEntityById = @"SELECT * FROM RaceGamblingType WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Type FROM RaceGamblingType";
        QueryComplexById = @"SELECT Id, Type FROM RaceGamblingType WHERE Id = @Id";
    }

    /// <inheritdoc />
    private protected override async Task<List<RaceGamblingTypeComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<RaceGamblingTypeComplex>(QueryComplex);
        return data.ToList();
    }

    /// <inheritdoc />
    private protected override async Task<RaceGamblingTypeComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<RaceGamblingTypeComplex>(QueryComplexById,
            new { Id = id });
        return data;
    }
}