    using Application.Configurations;
    using Application.DatasetBuilder.Exceptions;
    using Application.DatasetBuilder.Interfaces;
    using Application.Repository.Interfaces;
    using Microsoft.Extensions.Options;
    using Models.Complex;
    using Models.Datasets;
    using Models.Entity;

    namespace Application.DatasetBuilder.Services;

    public class DatasetBuilderService<T>(
        IOptions<DatasetBuilderRules> rules,
        IReadAllRepository<CompetitionEntity, CompetitionComplex> competitionRepository,
        IReadAllRepository<RaceCourseEntity, RaceCourseComplex> raceCourseRepository,
        IReadSourceRepository<DriverEntity, DriverComplex> driverRepository,
        IReadSourceRepository<HorseEntity, HorseComplex> horseRepository,
        IRaceRepository raceRepository,
        IRaceParticipantRepository raceParticipantRepository,
        IRaceResultRepository raceResultRepository)
        : IDatasetBuilderService<T>

    {
        private RaceComplex _race = null!;
        protected List<RaceParticipantComplex> Participants = null!;

        public virtual Task<List<T>> BuildAsync(string raceId)
        {
            var message = "No functionality added to build method";
            throw new DatasetNoBuildMethodException(message);
        }

        protected async Task InitializeRaceAsync(string raceId)
        {
            _race = await GetRaceComplexDataAsync(raceId);
            Participants = _race.Participants;
        }

        protected async Task<DatasetBasic> BuildBasicDataAsync(string raceId)
        {
            var raceEntity = await GetRaceEntityDataAsync(raceId);
            var competitionEntity = await GetCompetitionEntityDataAsync(raceEntity.CompetitionId);
            var raceCourseEntity = await raceCourseRepository.GetEntityByIdAsync(competitionEntity.RaceCourseId);

            var data = new DatasetBasic
            {
                Date = competitionEntity.Date,
                RaceCourse = raceCourseEntity!.Name,
                RaceNumber = _race.RaceNumber,
                HorseType = _race.HorseType?.Type,
                StartType = _race.StartType.Type,
                Monte = _race.Monte
            };

            return data;
        }

        protected bool CheckRules()
        {
            if (Participants.Count < rules.Value.MinimumParticipants) return false;
            return true;
        }

        protected async Task<DriverComplex> GetDriverAsync(string sourceId)
        {
            var data = await driverRepository.GetComplexBySourceIdAsync(sourceId);
            if (data != null) return data;
            var errorMsg = $"Driver entity was expected for source id {sourceId}";
            throw new DatasetNoParticipantFoundException(errorMsg);
        }

        protected async Task<HorseComplex> GetHorseAsync(string sourceId)
        {
            var data = await horseRepository.GetComplexBySourceIdAsync(sourceId);
            if (data != null) return data;
            var errorMsg = $"Horse entity was expected for id {sourceId}";
            throw new DatasetNoParticipantFoundException(errorMsg);
        }


        protected async Task<List<RaceParticipantComplex>> GetRaceParticipantByDriverAsync(string sourceId)
        {
            var data = await raceParticipantRepository.GetComplexByDriverAsync(sourceId);
            if (data != null) return data;
            var errorMsg = $"Race participant entity was expected for Driver: {sourceId}";
            throw new DatasetNoParticipantFoundException(errorMsg);
        }

        protected async Task<List<RaceParticipantComplex>> GetRaceParticipantByHorseAsync(string sourceId)
        {
            var data = await raceParticipantRepository.GetComplexesByHorseAsync(sourceId);
            if (data != null) return data;
            var errorMsg = $"Race participant entity was expected for Horse: {sourceId}";
            throw new DatasetNoParticipantFoundException(errorMsg);
        }

        protected async Task<RaceResultComplex> GetRaceResultByParticipant(string participantId)
        {
            var data = await raceResultRepository.GetComplexByParticipantIdAsync(participantId);
            if (data != null) return data;
            var errorMsg = $"Race result was expected for participantId {participantId}";
            throw new DatasetResultNotFound(errorMsg);
        }


        private async Task<RaceComplex> GetRaceComplexDataAsync(string raceId)
        {
            var data = await raceRepository.GetComplexByIdAsync(raceId);
            if (data != null) return data;
            var errorMsg = $"Race entity was expected for id {raceId}";
            throw new DatasetRaceNotFoundException(errorMsg);
        }

        private async Task<RaceEntity> GetRaceEntityDataAsync(string raceId)
        {
            var data = await raceRepository.GetEntityByIdAsync(raceId);
            if (data != null) return data;
            var errorMsg = $"Race entity was expected for id {raceId}";
            throw new DatasetRaceNotFoundException(errorMsg);
        }

        private async Task<CompetitionEntity> GetCompetitionEntityDataAsync(string competitionId)
        {
            var data = await competitionRepository.GetEntityByIdAsync(competitionId);
            if (data != null) return data;
            var errorMsg = $"Competition entity was expected for id {competitionId}";
            throw new DatasetRaceNotFoundException(errorMsg);
        }
    }