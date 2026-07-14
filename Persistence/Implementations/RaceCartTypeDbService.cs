using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

public class RaceCartTypeDbService : DbService<RaceCartTypeEntity, RaceCartTypeComplex>
{
    public RaceCartTypeDbService(IConfiguration configuration, ModelPolicy<RaceCartTypeEntity> policy)
        : base(configuration, policy)
    {
        QueryIds = @"SELECT Id FROM RaceCartType";
        QuerySimple = @"SELECT * FROM RaceCartType";
        QuerySimpleById = @"SELECT * FROM RaceCartType WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Type FROM RaceCartType";
        QueryComplexById = @"SELECT Id, Type FROM RaceCartType WHERE Id = @Id";
    }

    private protected override async Task<List<RaceCartTypeComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<RaceCartTypeComplex>(QueryComplex);
        return data.ToList();
    }

    private protected override async Task<RaceCartTypeComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<RaceCartTypeComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}