using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

public class RaceGamblingTypeDbService : DbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>
{
    public RaceGamblingTypeDbService(IConfiguration configuration, ModelPolicy<RaceGamblingTypeEntity> policy)
        : base(configuration, policy)
    {
        QueryIds = @"SELECT Id FROM RaceGamblingType";
        QuerySimple = @"SELECT * FROM RaceGamblingType";
        QuerySimpleById = @"SELECT * FROM RaceGamblingType WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Type FROM RaceGamblingType";
        QueryComplexById = @"SELECT Id, Type FROM RaceGamblingType WHERE Id = @Id";
    }

    private protected override async Task<List<RaceGamblingTypeComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<RaceGamblingTypeComplex>(QueryComplex);
        return data.ToList();
    }

    private protected override async Task<RaceGamblingTypeComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<RaceGamblingTypeComplex>(QueryComplexById,
            new { Id = id });
        return data;
    }
}