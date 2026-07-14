using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <inheritdoc />
public class DriverLicenseDbService : DbService<DriverLicenseEntity, DriverLicenseComplex>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DriverLicenseDbService" /> class
    ///     and configures the specific SQL queries for Driver License entities.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    public DriverLicenseDbService(IConfiguration configuration)
        : base(configuration)
    {
        QueryIds = @"SELECT Id FROM DriverLicense";
        QueryEntity = @"SELECT * FROM DriverLicense ORDER BY Code";
        QueryEntityById = @"SELECT * FROM DriverLicense WHERE Id = @Id ORDER BY Code";
        QueryComplex = @"SELECT Id, Code, Description FROM DriverLicense ORDER BY Code";
        QueryComplexById = @"SELECT Id, Code, Description FROM DriverLicense WHERE Id = @Id ORDER BY Code";
    }

    /// <inheritdoc />
    private protected override async Task<List<DriverLicenseComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<DriverLicenseComplex>(QueryComplex);
        return data.ToList();
    }

    /// <inheritdoc />
    private protected override async Task<DriverLicenseComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<DriverLicenseComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}