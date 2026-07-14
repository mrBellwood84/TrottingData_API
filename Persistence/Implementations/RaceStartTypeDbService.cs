using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <inheritdoc />
public class RaceStartTypeDbService : DbService<RaceStartTypeEntity, RaceStartTypeComplex>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RaceStartTypeDbService" /> class
    ///     and configures the specific SQL queries for Race Start Type entities.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public RaceStartTypeDbService(IConfiguration configuration)
        : base(configuration)
    {
        QueryIds = @"SELECT Id FROM RaceStartType";
        QueryEntity = @"SELECT * FROM RaceStartType";
        QueryEntityById = @"SELECT * FROM RaceStartType WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Type FROM RaceStartType";
        QueryComplexById = @"SELECT Id, Type FROM RaceStartType WHERE Id = @Id";
    }

    /// <inheritdoc />
    private protected override async Task<List<RaceStartTypeComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<RaceStartTypeComplex>(QueryComplex);
        return data.ToList();
    }

    /// <inheritdoc />
    private protected override async Task<RaceStartTypeComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<RaceStartTypeComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}