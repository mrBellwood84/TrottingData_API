using Application.Cache.Interfaces;
using Application.Repository.Services;
using Models.Complex;
using Models.Entity;
using Models.Shared;
using Persistence.Interfaces;

namespace Application.Repository.Implementations;

public class CompetitionRepository(
    IReadAllCache<CompetitionEntity> entityCache,
    IReadAllCache<CompetitionComplex> complexCache,
    IReadAllDbService<CompetitionEntity, CompetitionComplex> dbService)
    : ReadAllRepository<CompetitionEntity, CompetitionComplex>(entityCache, complexCache, dbService)
{
    protected override ModelPolicy ModelPolicy { get; } = new() { AllowIdList = true, AllowGetAll = false };
}