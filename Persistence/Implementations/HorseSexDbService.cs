using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

public class HorseSexDbService : DbService<HorseSexEntity, HorseSexComplex>
{
    public HorseSexDbService(IConfiguration configuration, ModelPolicy<HorseSexEntity> policy)
        : base(configuration, policy)
    {
        QueryIds = @"SELECT Id FROM HorseSex";
        QuerySimple = @"SELECT * FROM HorseSex";
        QuerySimpleById = @"SELECT * FROM HorseSex WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Sex FROM HorseSex";
        QueryComplexById = @"SELECT Id, Sex FROM HorseSex WHERE Id = @Id";
    }

    private protected override async Task<List<HorseSexComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<HorseSexComplex>(QueryComplex);
        return data.ToList();
    }

    private protected override async Task<HorseSexComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync(QueryComplexById, new { Id = id });
        return data;
    }
}