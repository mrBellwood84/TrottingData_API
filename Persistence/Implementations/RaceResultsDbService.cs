using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Database service for handling race results.
///     Provides data access operations for retrieving both flat <see cref="RaceResultEntity" />
///     structures and rich <see cref="RaceResultsComplex" /> models by primary ID or participant ID.
/// </summary>
public sealed class RaceResultDbService(IConfiguration configuration)
    : ReadSingleDbService<RaceResultEntity, RaceResultsComplex>(configuration), IRaceResultDbService
{
    // Query to retrieve a flat entity based on the associated race participant identifier
    private const string SqlSelectEntityByParticipantId =
        @"SELECT * FROM RaceResults WHERE RaceParticipantId = @RaceParticipantId";

    // Query to retrieve a complex model based on the associated race participant identifier
    private const string SqlSelectComplexByParticipantId =
        @"SELECT * FROM RaceResults WHERE RaceParticipantId = @RaceParticipantId";

    // Simple query to retrieve a flat race result entity by its unique identifier
    protected override string SqlSelectEntityById =>
        @"SELECT * FROM RaceResults WHERE Id = @Id";

    // Simple query to retrieve a complex race result model by its unique identifier
    protected override string SqlSelectComplexById =>
        @"SELECT * FROM RaceResults WHERE Id = @Id";

    /// <summary>
    ///     Retrieves a single flat race result entity by its associated race participant identifier.
    /// </summary>
    /// <param name="participantId">The unique identifier of the race participant.</param>
    /// <returns>The flat entity representation of the race result if found; otherwise, null.</returns>
    public Task<RaceResultEntity?> GetEntityByParticipantIdAsync(string participantId)
    {
        return QueryEntityAsync(SqlSelectEntityByParticipantId, new { RaceParticipantId = participantId });
    }

    /// <summary>
    ///     Retrieves a single complex race result model by its associated race participant identifier.
    /// </summary>
    /// <param name="participantId">The unique identifier of the race participant.</param>
    /// <returns>The complex representation of the race result if found; otherwise, null.</returns>
    public Task<RaceResultsComplex?> GetComplexByParticipantIdAsync(string participantId)
    {
        return QueryComplexAsync(SqlSelectComplexByParticipantId, new { RaceParticipantId = participantId });
    }
}