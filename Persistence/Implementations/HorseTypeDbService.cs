using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Services;

namespace Persistence.Implementations;

/// <inheritdoc />
public class HorseTypeDbService : DbService<HorseTypeEntity, HorseTypeComplex>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HorseTypeDbService"/> class
    /// and configures the specific SQL queries for Horse Type entities.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="policy">The policy governing access to horse type entities.</param>
    public HorseTypeDbService(IConfiguration configuration, ModelPolicy<HorseTypeEntity> policy)
        : base(configuration, policy)
    {
        QueryIds = @"SELECT Id FROM HorseType";
        QuerySimple = @"SELECT * FROM HorseType";
        QuerySimpleById = @"SELECT * FROM HorseType WHERE Id = @Id";
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