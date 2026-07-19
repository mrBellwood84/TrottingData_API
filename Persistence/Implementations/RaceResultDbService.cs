using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Database service for handling race results.
///     Manages single-instance reads for both flat entities and complex result models.
///     Since the result structure contains only baseline value fields, it maps directly
///     via Dapper without the need for multi-join hydration pipelines.
/// </summary>
public sealed class RaceResultDbService(IConfiguration configuration)
    : ReadSingleDbService<RaceResultEntity, RaceResultComplex>(configuration), IRaceResultDbService
{
    protected override string SqlSelectEntityById => @"SELECT * FROM RaceResults WHERE Id = @Id";
    private string SqlSelectEntityByParticipantId => @"SELECT * FROM RaceResults WHERE RaceParticipantId = @Id";

    protected override string SqlSelectComplexById => @"SELECT * FROM RaceResults WHERE Id = @Id";
    private string SqlSelectComplexByParticipantId => @"SELECT * FROM RaceResults WHERE RaceParticipantId = @Id";

    /// <summary>
    ///     Retrieves a single flat race result entity associated with a specific race participant ID.
    /// </summary>
    public Task<RaceResultEntity?> GetEntityByParticipantIdAsync(string participantId)
    {
        return QueryEntityAsync(SqlSelectEntityByParticipantId, new { Id = participantId });
    }

    /// <summary>
    ///     Retrieves a single complex race result model associated with a specific race participant ID.
    /// </summary>
    public Task<RaceResultComplex?> GetComplexByParticipantIdAsync(string participantId)
    {
        return QueryComplexAsync(SqlSelectComplexByParticipantId, new { Id = participantId });
    }
}