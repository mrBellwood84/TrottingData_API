using Microsoft.Extensions.Configuration;
using Models.Complex;
using Models.Entity;
using Persistence.Services;

namespace Persistence.Implementations;

/// <summary>
///     Provides database access operations for race courses, handling both flat
///     <see cref="RaceCourseEntity" /> structures and rich <see cref="RaceCourseComplex" /> models.
///     Data is consistently ordered by the course name.
/// </summary>
public sealed class RaceCourseDbService(IConfiguration configuration)
    : ReadAllDbService<RaceCourseEntity, RaceCourseComplex>(configuration)
{
    protected override string SqlSelectIds =>
        @"SELECT Id FROM RaceCourse";

    protected override string SqlSelectEntities =>
        @"SELECT * FROM RaceCourse ORDER BY Name";

    protected override string SqlSelectEntityById =>
        @"SELECT * FROM RaceCourse WHERE Id = @Id ORDER BY Name";

    protected override string SqlSelectComplex =>
        @"SELECT Id, Name FROM RaceCourse ORDER BY Name";

    protected override string SqlSelectComplexById =>
        @"SELECT Id, Name FROM RaceCourse WHERE Id = @Id ORDER BY Name";
}