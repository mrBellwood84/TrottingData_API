using Models.Entities;
using Models.Interfaces;

namespace Application.Repository.Interfaces;

public interface IRepositoryService<TSimple, TComplex> where TSimple : IDbItem where TComplex : IDbItem
{
    Task<List<IdModel>> GetIdsAsync();
    Task<List<TSimple>> GetAllSimplesAsync();
    Task<TSimple?> GetSimpleByIdAsync(string id);
    Task<List<TComplex>> GetAllComplexAsync();
    Task<TComplex?> GetComplexByIdAsync(string id);
}