using Models.Entities;

namespace Persistence.Interfaces;

public interface IDbService<TSimple, TComplex>
{
    Task<List<IdModel>> GetIdsAsync();
    Task<List<TSimple>> GetAllSimpleAsync();
    Task<TSimple?> GetSimpleByIdAsync(string id);
    Task<List<TComplex>> GetAllComplexAsync();
    Task<TComplex?> GetComplexByIdAsync(string id);
}