using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Interfaces;
using Persistence.Services;

namespace Persistence.Implementations;


public sealed class RaceResultDbService(IConfiguration configuration)
    : ReadSingleDbService<RaceResultEntity, RaceResultsComplex>(configuration)
{
    protected override string SqlSelectEntityById => @"SELECT * FROM RaceResults WHERE Id = @Id";
    private string SqlSelectEntityByParticipantId => @"SELECT * FROM RaceResults WHERE RaceParticipantId = @Id";
    
    protected override string SqlSelectComplexById => @"SELECT * FROM RaceResults WHERE Id = @Id";
    private string SqlSelectComplexByParticipantId => @"SELECT * FROM RaceResults WHERE RaceParticipantId = @Id";
    
    public Task<RaceResultEntity?> GetEntityByParticipantIdAsync(string participantId)
    {
        return QueryEntityAsync(SqlSelectEntityByParticipantId, new { Id = participantId });
    }

    public Task<RaceResultsComplex?> GetComplexByParticipantIdAsync(string participantId)
    {
        return QueryComplexAsync(SqlSelectComplexByParticipantId, new { Id = participantId });
    }
}