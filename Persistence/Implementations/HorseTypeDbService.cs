using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for horse types, handling both flat
///     <see cref="HorseTypeEntity" /> structures and rich <see cref="HorseTypeComplex" /> models.
/// </summary>
public sealed class HorseTypeDbService(IConfiguration configuration)
    : ReadAllDbService<HorseTypeEntity, HorseTypeComplex>(configuration)
{
    protected override string SqlSelectIds =>
        @"SELECT Id FROM HorseType";

    protected override string SqlSelectEntities =>
        @"SELECT * FROM HorseType";

    protected override string SqlSelectEntityById =>
        @"SELECT * FROM HorseType WHERE Id = @Id";

    protected override string SqlSelectComplex =>
        @"SELECT Id, Type FROM HorseType";

    protected override string SqlSelectComplexById =>
        @"SELECT Id, Type FROM HorseType WHERE Id = @Id";
}