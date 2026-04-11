using bowling_tournament_MVCPRoject.Domain.Daos;
using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
using bowling_tournament_MVCPRoject.Domain.Dtos.Results;
using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentDao _tournamentDao;

        public TournamentService(ITournamentDao tournamentDao)
        {
            _tournamentDao = tournamentDao;
        }

        public TournamentResult tryRegisterTournament(TournamentRequest request)
        {
            TournamentResult result = validateTournamentRequest(request, isUpdate: false);

            if (!result.success)
            {
                return result;
            }

            Tournament tournament = new Tournament
            {
                TournamentName = request.Name.Trim(),
                WatcherCapacity = request.WatcherCapacity,
                TournamentDate = request.DateOfGame,
                Location = request.Location.Trim(),
                TeamCapacity = request.TeamCapacity,
                RegistrationOpen = request.RegistrationOpen,
                MensCapacity = request.MensCapacity,
                WomensCapacity = request.WomensCapacity,
                MixedCapacity = request.MixedCapacity,
                YouthCapacity = request.YouthCapacity,
                SeniorCapacity = request.SeniorCapacity
            };

            _tournamentDao.addTournament(tournament);

            result.success = true;
            return result;
        }

        public TournamentResult tryUpdateTournament(TournamentRequest request)
        {
            TournamentResult result = validateTournamentRequest(request, isUpdate: true);

            if (!result.success)
            {
                return result;
            }

            Tournament existingTournament = _tournamentDao.findTournament(
                new Tournament { TournamentId = request.Id }
            );

            if (existingTournament == null || existingTournament.TournamentId <= 0)
            {
                result.Errors.Add("Tournament not found.");
                result.success = false;
                return result;
            }

            existingTournament.TournamentName = request.Name.Trim();
            existingTournament.WatcherCapacity = request.WatcherCapacity;
            existingTournament.TournamentDate = request.DateOfGame;
            existingTournament.Location = request.Location.Trim();
            existingTournament.TeamCapacity = request.TeamCapacity;
            existingTournament.RegistrationOpen = request.RegistrationOpen;

            existingTournament.MensCapacity = request.MensCapacity;
            existingTournament.WomensCapacity = request.WomensCapacity;
            existingTournament.MixedCapacity = request.MixedCapacity;
            existingTournament.YouthCapacity = request.YouthCapacity;
            existingTournament.SeniorCapacity = request.SeniorCapacity;

            _tournamentDao.saveChanges();

            result.success = true;
            return result;
        }

        private TournamentResult validateTournamentRequest(TournamentRequest request, bool isUpdate)
        {
            TournamentResult result = new TournamentResult();

            if (request == null)
            {
                result.Errors.Add("Request cannot be null.");
            }
            else
            {
                if (isUpdate && request.Id <= 0)
                {
                    result.Errors.Add("A valid tournament ID is required for update.");
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    result.Errors.Add("Tournament name is required.");
                }

                if (string.IsNullOrWhiteSpace(request.Location))
                {
                    result.Errors.Add("Location is required.");
                }

                if (request.TeamCapacity <= 0)
                {
                    result.Errors.Add("Team capacity must be greater than 0.");
                }

                if (request.WatcherCapacity < 0)
                {
                    result.Errors.Add("Watcher capacity cannot be negative.");
                }

                if (request.DateOfGame == default)
                {
                    result.Errors.Add("Tournament date is required.");
                }
            }

            result.success = result.Errors.Count == 0;
            return result;
        }
    }
}
