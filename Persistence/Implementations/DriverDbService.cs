using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Concrete implementation of <see cref="SourcedDbService{TEntity, TComplex}"/> for managing <see cref="DriverEntity"/> 
///     and assembling the aggregated <see cref="DriverComplex"/> model.
/// </summary>
public class DriverDbService : SourcedDbService<DriverEntity, DriverComplex>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DriverDbService"/> class, 
    ///     configuring the specific SQL queries required for driver data operations.
    /// </summary>
    /// <param name="configuration">The application configuration for database connections.</param>
    /// <param name="policy">The access and retrieval policy governing driver entities.</param>
    public DriverDbService(IConfiguration configuration, ModelPolicy<DriverEntity> policy)
        : base(configuration, policy)
    {
        // Flat / simple queries mapping 1:1 with the Driver table
        QueryIds = @"SELECT Id FROM Driver LIMIT 10";
        QueryEntityById = @"SELECT * FROM Driver WHERE Id = @Id";
        QueryEntityBySourceId = @"SELECT * FROM Driver WHERE SourceId = @SourceId";

        // Complex queries requiring SQL Joins to stitch together the complete domain model
        QueryComplexById = @"SELECT * FROM Driver AS d 
                                JOIN DriverLicense AS dl on d.DriverLicenseId = dl.Id
                                WHERE d.Id = @Id";

        QueryComplexBySourceId = @"SELECT * FROM Driver AS d 
                                JOIN DriverLicense AS dl on d.DriverLicenseId = dl.Id
                                WHERE d.SourceId = @SourceId";
    }

    /// <summary>
    ///     Queries the database and maps a flat driver record and its license record 
    ///     into a unified <see cref="DriverComplex"/> object.
    /// </summary>
    /// <param name="query">The SQL query to execute (must include a JOIN with DriverLicense).</param>
    /// <param name="parameters">The anonymous parameter object containing query values (e.g., ID or SourceID).</param>
    /// <returns>A fully populated <see cref="DriverComplex"/> instance, or <see langword="null"/> if no match was found.</returns>
    private async Task<DriverComplex?> QueryAndMapComplexAsync(string query, object parameters)
    {
        await using var connection = await CreateConnection();
        
        // Multi-mapping: Dapper splits each row into a DriverComplex and a DriverLicenseComplex,
        // then passes them into the lambda where they are stitched together.
        var data = await connection
            .QueryAsync<DriverComplex, DriverLicenseComplex, DriverComplex>(
                query,
                (driver, license) =>
                {
                    // Map the nested object directly to the complex parent
                    driver.License = license;
                    return driver;
                },
                parameters,
                splitOn: "Id"); // Tells Dapper where the DriverLicense columns start in the SELECT clause
        
        return data.FirstOrDefault();
    }

    /// <summary>
    ///     Core database logic to retrieve a complex driver representation by its internal database identifier.
    /// </summary>
    /// <param name="id">The GUID string of the driver.</param>
    /// <returns>A task representing the asynchronous database lookup and mapping operation.</returns>
    private protected override Task<DriverComplex?> GetComplexByIdLogicAsync(string id)
    {
        var param = new { Id = id };
        
        // DIRECT TASK RETURN (Elision): Returning the Task directly without await 
        // avoids the performance overhead of creating an async state machine.
        return QueryAndMapComplexAsync(QueryComplexById, param);
    }

    /// <summary>
    ///     Core database logic to retrieve a complex driver representation by its external source identifier.
    /// </summary>
    /// <param name="sourceId">The external system identifier (e.g., Rikstoto client ID).</param>
    /// <returns>A task representing the asynchronous database lookup and mapping operation.</returns>
    private protected override Task<DriverComplex?> GetComplexBySourceIdLogicAsync(string sourceId)
    {
        var param = new { SourceId = sourceId };
        
        // Direct task return used here as well for optimal performance
        return QueryAndMapComplexAsync(QueryComplexBySourceId, param);
    }
}