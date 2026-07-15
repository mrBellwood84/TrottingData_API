using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for horse sexes, handling both flat
///     <see cref="HorseSexEntity" /> structures and rich <see cref="HorseSexComplex" /> models.
/// </summary>
public sealed class HorseSexDbService(IConfiguration configuration)
    : ReadAllDbService<HorseSexEntity, HorseSexComplex>(configuration)
{
    protected override string SqlSelectIds =>
        @"SELECT Id FROM HorseSex";

    protected override string SqlSelectEntities =>
        @"SELECT * FROM HorseSex";

    protected override string SqlSelectEntityById =>
        @"SELECT * FROM HorseSex WHERE Id = @Id";

    protected override string SqlSelectComplex =>
        @"SELECT Id, Sex FROM HorseSex";

    protected override string SqlSelectComplexById =>
        @"SELECT Id, Sex FROM HorseSex WHERE Id = @Id";
}