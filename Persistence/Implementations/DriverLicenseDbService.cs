using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for driver licenses, handling both flat
///     <see cref="DriverLicenseEntity" /> structures and rich <see cref="DriverLicenseComplex" /> models.
///     Data is consistently ordered by the license code.
/// </summary>
/// <param name="configuration">The application configuration containing connection string definitions.</param>
public sealed class DriverLicenseDbService(IConfiguration configuration)
    : ReadAllDbService<DriverLicenseEntity, DriverLicenseComplex>(configuration)
{
    protected override string SqlSelectIds =>
        @"SELECT Id FROM DriverLicense";

    protected override string SqlSelectEntities =>
        @"SELECT * FROM DriverLicense ORDER BY Code";

    protected override string SqlSelectEntityById =>
        @"SELECT * FROM DriverLicense WHERE Id = @Id ORDER BY Code";

    protected override string SqlSelectComplex =>
        @"SELECT Id, Code, Description FROM DriverLicense ORDER BY Code";

    protected override string SqlSelectComplexById =>
        @"SELECT Id, Code, Description FROM DriverLicense WHERE Id = @Id ORDER BY Code";
}