using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Concrete implementation of <see cref="SourcedDbService{TEntity, TComplex}"/> for managing <see cref="HorseEntity"/> 
///     and assembling the aggregated <see cref="HorseComplex"/> model.
/// </summary>
public class HorseDbService : SourcedDbService<HorseEntity, HorseComplex>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HorseDbService"/> class, 
    ///     configuring the specific SQL queries required for horse data operations.
    /// </summary>
    /// <param name="configuration">The application configuration for database connections.</param>
    /// <param name="policy">The access and retrieval policy governing horse entities.</param>
    public HorseDbService(IConfiguration configuration, ModelPolicy<HorseEntity> policy)
        : base(configuration, policy)
    {
        // Flat / simple queries mapping 1:1 with the Horse table
        QueryEntityById = @"SELECT * FROM Horse WHERE Id = @Id";
        QueryEntityBySourceId = @"SELECT * FROM Horse WHERE SourceId = @SourceId";

        // Complex queries joining both sex and type info to build the complete horse model
        QueryComplexById = @"SELECT * FROM Horse AS h
                    LEFT JOIN HorseSex AS hs ON h.HorseSexId = hs.Id
                    LEFT JOIN HorseType AS ht ON h.HorseTypeId = ht.Id 
                    WHERE h.Id = @Id";

        QueryComplexBySourceId = @"SELECT * FROM Horse AS h
                          LEFT JOIN HorseSex AS hs ON h.HorseSexId = hs.Id
                          LEFT JOIN HorseType AS ht ON h.HorseTypeId = ht.Id 
                          WHERE h.SourceId = @SourceId";
    }

    /// <summary>
    ///     Queries the database and maps a flat horse record, its sex definition, and its type definition
    ///     into a unified, nested <see cref="HorseComplex"/> object.
    /// </summary>
    /// <param name="query">The SQL query to execute (must include JOINs with both HorseSex and HorseType).</param>
    /// <param name="parameters">The anonymous parameter object containing query values (e.g., ID or SourceID).</param>
    /// <returns>A fully populated <see cref="HorseComplex"/> instance, or <see langword="null"/> if no match was found.</returns>
    private async Task<HorseComplex?> QueryAndMapComplexAsync(string query, object parameters)
    {
        await using var connection = await CreateConnection();
        
        // Multi-mapping: Dapper maps each row to three distinct objects: HorseComplex, HorseSexComplex, and HorseTypeComplex.
        // The lambda expression then wires up the relationships before returning the parent horse object.
        var data = await connection
            .QueryAsync<HorseComplex, HorseSexComplex?, HorseTypeComplex?, HorseComplex>(
                query,
                (horse, sex, type) =>
                {
                    // Map the nested objects directly to the complex parent
                    horse.Sex = sex;
                    horse.Type = type;
                    return horse;
                },
                parameters,
                splitOn: "Id"); // Tells Dapper to split the columns into separate objects every time it encounters an "Id" column

        return data.FirstOrDefault();
    }

    /// <summary>
    ///     Core database logic to retrieve a complex horse representation by its internal database identifier.
    /// </summary>
    /// <param name="id">The GUID string of the horse.</param>
    /// <returns>A task representing the asynchronous database lookup and mapping operation.</returns>
    private protected override Task<HorseComplex?> GetComplexByIdLogicAsync(string id)
    {
        var param = new { Id = id };
        
        // DIRECT TASK RETURN (Elision): Returning the Task directly without await 
        // avoids the performance overhead of creating an async state machine.
        return QueryAndMapComplexAsync(QueryComplexById, param);
    }

    /// <summary>
    ///     Core database logic to retrieve a complex horse representation by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external system identifier (e.g., Rikstoto horse ID).</param>
    /// <returns>A task representing the asynchronous database lookup and mapping operation.</returns>
    private protected override Task<HorseComplex?> GetComplexBySourceIdLogicAsync(string sourceId)
    {
        var param = new { SourceId = sourceId };
        
        // Direct task return used here as well for optimal performance
        return QueryAndMapComplexAsync(QueryComplexBySourceId, param);
    }
}