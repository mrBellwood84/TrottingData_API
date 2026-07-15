using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for race cart types, handling both flat
///     <see cref="RaceCartTypeEntity" /> structures and rich <see cref="RaceCartTypeComplex" /> models.
/// </summary>
public sealed class RaceCartTypeDbService(IConfiguration configuration)
    : ReadAllDbService<RaceCartTypeEntity, RaceCartTypeComplex>(configuration)
{
    protected override string SqlSelectIds =>
        @"SELECT Id FROM RaceCartType";

    protected override string SqlSelectEntities =>
        @"SELECT * FROM RaceCartType";

    protected override string SqlSelectEntityById =>
        @"SELECT * FROM RaceCartType WHERE Id = @Id";

    protected override string SqlSelectComplex =>
        @"SELECT Id, Type FROM RaceCartType";

    protected override string SqlSelectComplexById =>
        @"SELECT Id, Type FROM RaceCartType WHERE Id = @Id";
}