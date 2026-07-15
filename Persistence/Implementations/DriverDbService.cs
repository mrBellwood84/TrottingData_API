using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for drivers, handling both flat
///     <see cref="DriverEntity" /> structures and rich <see cref="DriverComplex" /> models
///     by stitching together drivers and their licenses.
/// </summary>
public sealed class DriverDbService(IConfiguration configuration)
    : ReadSourcedDbService<DriverEntity, DriverComplex>(configuration)
{
    protected override string SqlSelectEntityById =>
        @"SELECT * FROM Driver WHERE Id = @Id";

    protected override string SqlSelectEntityBySourceId =>
        @"SELECT * FROM Driver WHERE SourceId = @SourceId";

    protected override string SqlSelectComplexById =>
        @"SELECT * FROM Driver AS d 
          LEFT JOIN DriverLicense AS dl on d.DriverLicenseId = dl.Id
          WHERE d.Id = @Id";

    protected override string SqlSelectComplexBySourceId =>
        @"SELECT * FROM Driver AS d 
          LEFT JOIN DriverLicense AS dl on d.DriverLicenseId = dl.Id
          WHERE d.SourceId = @SourceId";

    protected override async Task<DriverComplex?> QueryComplexAsync(string sql, object? param = null)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<DriverComplex, DriverLicenseComplex?, DriverComplex>(
            sql,
            (driver, license) =>
            {
                driver.License = license;
                return driver;
            },
            param,
            splitOn: "Id");

        return data.FirstOrDefault();
    }
}