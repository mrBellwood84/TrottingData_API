using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <inheritdoc />
public class HorseTypeDbService : DbService<HorseTypeEntity, HorseTypeComplex>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HorseTypeDbService" /> class
    ///     and configures the specific SQL queries for Horse Type entities.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public HorseTypeDbService(IConfiguration configuration)
        : base(configuration)
    {
        QueryIds = @"SELECT Id FROM HorseType";
        QueryEntity = @"SELECT * FROM HorseType";
        QueryEntityById = @"SELECT * FROM HorseType WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Type FROM HorseType";
        QueryComplexById = @"SELECT Id, Type FROM HorseType WHERE Id = @Id";
    }

    /// <inheritdoc />
    private protected override async Task<List<HorseTypeComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<HorseTypeComplex>(QueryComplex);
        return data.ToList();
    }

    /// <inheritdoc />
    private protected override async Task<HorseTypeComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<HorseTypeComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}