using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

public class DriverLicenseDbService : DbService<DriverLicenseEntity, DriverLicenseComplex>
{
    public DriverLicenseDbService(IConfiguration configuration) : base(configuration)
    {
        QueryIds = @"SELECT Id FROM DriverLicense";
        QuerySimple = @"SELECT * FROM DriverLicense";
        QuerySimpleById = @"SELECT Id FROM DriverLicense WHERE Id = @Id";
        QueryComplex = @"SELECT Id, Code, Description FROM DriverLicense";
        QueryComplexById = @"SELECT Id, Code, Description FROM DriverLicense WHERE Id = @Id";
        
        AllowAllSimpleQuery = true;
        AllowAllComplexQuery = true;
    }

    internal override async Task<List<DriverLicenseComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<DriverLicenseComplex>(QueryComplex);
        return data.ToList();
    }

    internal override async Task<DriverLicenseComplex> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstAsync<DriverLicenseComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}