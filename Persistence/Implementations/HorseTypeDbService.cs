using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

public class HorseTypeDbService : DbService<HorseTypeEntity, HorseTypeComplex>
{
    public HorseTypeDbService(IConfiguration configuration, ModelPolicy<HorseTypeEntity> policy)
        : base(configuration, policy)
    {
        QueryIds = @"SELECT Id FROM HorseType";
        QuerySimple = @"SELECT * FROM HorseType";
        QuerySimpleById = @"SELECT * FROM HorseType WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Type FROM HorseType";
        QueryComplexById = @"SELECT Id, Type FROM HorseType WHERE Id = @Id";
    }

    private protected override async Task<List<HorseTypeComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<HorseTypeComplex>(QueryComplex);
        return data.ToList();
    }

    private protected override async Task<HorseTypeComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<HorseTypeComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}