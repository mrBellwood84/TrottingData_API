using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

public class DriverLicenseDbService : DbService<DriverLicenseEntity, DriverLicenseComplex>
{
    public DriverLicenseDbService(IConfiguration configuration, EntityPolicy<DriverLicenseEntity> policy) : base(
        configuration, policy)
    {
        QueryIds = @"SELECT Id FROM DriverLicense";
        QuerySimple = @"SELECT * FROM DriverLicense";
        QuerySimpleById = @"SELECT * FROM DriverLicense WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Code, Description FROM DriverLicense";
        QueryComplexById = @"SELECT Id, Code, Description FROM DriverLicense WHERE Id = @Id";
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