using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entities;
using Models.Simple;
using Persistence.Services;

namespace Persistence.Implementations;

/// <inheritdoc />
public class RaceCourseDbService : DbService<RaceCourseEntity, RaceCourseComplex>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RaceCourseDbService"/> class
    /// and configures the specific SQL queries for Race Course entities.
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="policy">The policy governing access to race course entities.</param>
    public RaceCourseDbService(IConfiguration configuration, ModelPolicy<RaceCourseEntity> policy)
        : base(configuration, policy)
    {
        QueryIds = @"SELECT Id FROM RaceCourse";
        QuerySimple = @"SELECT * FROM RaceCourse ORDER BY Name";
        QuerySimpleById = @"SELECT * FROM RaceCourse WHERE Id = @Id ORDER BY Name";
        QueryComplex = @"SELECT Id, Name FROM RaceCourse ORDER BY Name";
        QueryComplexById = @"SELECT Id, Name FROM RaceCourse WHERE Id = @Id ORDER BY Name";
    }

    /// <inheritdoc />
    private protected override async Task<List<RaceCourseComplex>> GetAllComplexLogicAsync()
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<RaceCourseComplex>(QueryComplex);
        return data.ToList();
    }

    /// <inheritdoc />
    private protected override async Task<RaceCourseComplex?> GetComplexByIdLogicAsync(string id)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryFirstOrDefaultAsync<RaceCourseComplex>(QueryComplexById, new { Id = id });
        return data;
    }
}