using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <inheritdoc />
public class HorseSexDbService : DbService<HorseSexEntity, HorseSexComplex>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HorseSexDbService" /> class
    ///     and configures the specific SQL queries for Horse Sex entities.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public HorseSexDbService(IConfiguration configuration)
        : base(configuration)
    {
        QueryIds = @"SELECT Id FROM HorseSex";
        QueryEntity = @"SELECT * FROM HorseSex";
        QueryEntityById = @"SELECT * FROM HorseSex WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Sex FROM HorseSex";
        QueryComplexById = @"SELECT Id, Sex FROM HorseSex WHERE Id = @Id";
    }

    /// <inheritdoc />
    private protected override async Task<List<HorseSexComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<HorseSexComplex>(QueryComplex);
        return data.ToList();
    }

    /// <inheritdoc />
    private protected override async Task<HorseSexComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();

        var data = await connection.QueryFirstOrDefaultAsync<HorseSexComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}