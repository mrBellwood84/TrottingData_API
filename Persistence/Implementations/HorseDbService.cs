using Dapper;
using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for horses, handling both flat
///     <see cref="HorseEntity" /> structures and rich <see cref="HorseComplex" /> models
///     by stitching together horses, their sex definitions, and their type definitions.
/// </summary>
public sealed class HorseDbService(IConfiguration configuration)
    : ReadSourcedDbService<HorseEntity, HorseComplex>(configuration)
{
    protected override string SqlSelectEntityById =>
        @"SELECT * FROM Horse WHERE Id = @Id";

    protected override string SqlSelectEntityBySourceId =>
        @"SELECT * FROM Horse WHERE SourceId = @SourceId";

    protected override string SqlSelectComplexById =>
        @"SELECT * FROM Horse AS h
          LEFT JOIN HorseSex AS hs ON h.HorseSexId = hs.Id
          LEFT JOIN HorseType AS ht ON h.HorseTypeId = ht.Id 
          WHERE h.Id = @Id";

    protected override string SqlSelectComplexBySourceId =>
        @"SELECT * FROM Horse AS h
          LEFT JOIN HorseSex AS hs ON h.HorseSexId = hs.Id
          LEFT JOIN HorseType AS ht ON h.HorseTypeId = ht.Id 
          WHERE h.SourceId = @SourceId";

    protected override async Task<HorseComplex?> QueryComplexAsync(string sql, object? param = null)
    {
        await using var connection = await CreateConnection();
        var data = await connection.QueryAsync<HorseComplex, HorseSexComplex?, HorseTypeComplex?, HorseComplex>(
            sql,
            (horse, sex, type) =>
            {
                horse.Sex = sex;
                horse.Type = type;
                return horse;
            },
            param,
            splitOn: "Id");

        return data.FirstOrDefault();
    }
}