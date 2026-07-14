using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

public class RaceStartTypeDbService : DbService<RaceStartTypeEntity, RaceStartTypeComplex>
{
    public RaceStartTypeDbService(IConfiguration configuration, ModelPolicy<RaceStartTypeEntity> modelPolicy)
        : base(configuration, modelPolicy)
    {
        QueryIds = @"SELECT Id FROM RaceStartType";
        QuerySimple = @"SELECT * FROM RaceStartType";
        QuerySimpleById = @"SELECT * FROM RaceStartType WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Type FROM RaceStartType";
        QueryComplexById = @"SELECT Id, Type FROM RaceStartType WHERE Id = @Id";
    }

    private protected override async Task<List<RaceStartTypeComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<RaceStartTypeComplex>(QueryComplex);
        return data.ToList();
    }

    private protected override async Task<RaceStartTypeComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<RaceStartTypeComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}