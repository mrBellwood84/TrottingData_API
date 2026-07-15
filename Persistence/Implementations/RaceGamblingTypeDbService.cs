using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for race gambling types, handling both flat
///     <see cref="RaceGamblingTypeEntity" /> structures and rich <see cref="RaceGamblingTypeComplex" /> models.
/// </summary>
public sealed class RaceGamblingTypeDbService(IConfiguration configuration)
    : ReadAllDbService<RaceGamblingTypeEntity, RaceGamblingTypeComplex>(configuration)
{
    protected override string SqlSelectIds =>
        @"SELECT Id FROM RaceGamblingType";

    protected override string SqlSelectEntities =>
        @"SELECT * FROM RaceGamblingType";

    protected override string SqlSelectEntityById =>
        @"SELECT * FROM RaceGamblingType WHERE Id = @Id";

    protected override string SqlSelectComplex =>
        @"SELECT Id, Type FROM RaceGamblingType";

    protected override string SqlSelectComplexById =>
        @"SELECT Id, Type FROM RaceGamblingType WHERE Id = @Id";
}