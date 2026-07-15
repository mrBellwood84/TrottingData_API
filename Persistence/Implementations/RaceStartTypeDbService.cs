using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for race start types, handling both flat
///     <see cref="RaceStartTypeEntity" /> structures and rich <see cref="RaceStartTypeComplex" /> models.
/// </summary>
public sealed class RaceStartTypeDbService(IConfiguration configuration)
    : ReadAllDbService<RaceStartTypeEntity, RaceStartTypeComplex>(configuration)
{
    protected override string SqlSelectIds =>
        @"SELECT Id FROM RaceStartType";

    protected override string SqlSelectEntities =>
        @"SELECT * FROM RaceStartType";

    protected override string SqlSelectEntityById =>
        @"SELECT * FROM RaceStartType WHERE Id = @Id";

    protected override string SqlSelectComplex =>
        @"SELECT Id, Type FROM RaceStartType";

    protected override string SqlSelectComplexById =>
        @"SELECT Id, Type FROM RaceStartType WHERE Id = @Id";
}