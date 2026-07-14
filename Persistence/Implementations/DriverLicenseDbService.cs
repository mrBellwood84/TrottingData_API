using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

public class DriverLicenseDbService : DbService<DriverLicenseEntity, DriverLicenseComplex>
{
    public DriverLicenseDbService(IConfiguration configuration, ModelPolicy<DriverLicenseEntity> policy)
        : base(configuration, policy)
    {
        QueryIds = @"SELECT Id FROM DriverLicense";
        QuerySimple = @"SELECT * FROM DriverLicense ORDER BY Code";
        QuerySimpleById = @"SELECT * FROM DriverLicense WHERE Id = @Id ORDER BY Code";
        QueryComplex = @"SELECT Id, Code, Description FROM DriverLicense ORDER BY Code";
        QueryComplexById = @"SELECT Id, Code, Description FROM DriverLicense WHERE Id = @Id ORDER BY Code";
    }

    private protected override async Task<List<DriverLicenseComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<DriverLicenseComplex>(QueryComplex);
        return data.ToList();
    }

    private protected override async Task<DriverLicenseComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<DriverLicenseComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}