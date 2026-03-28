using bowling_tournament_MVCPRoject.Domain.Daos;
using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
using bowling_tournament_MVCPRoject.Domain.Dtos.Results;
using bowling_tournament_MVCPRoject.Domain.Entities;
using bowling_tournament_MVCPRoject.Persistence.Daos;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public class TeamManagerService : ITeamManagerService
    {
        private readonly ITeamDao _teamDao;
        private readonly IPlayerDao _playerDao;
        private readonly ITournamentRegistrationDao _registrationDao;
        private readonly ITournamentDao _tournamentDao;

        public TeamManagerService(ITeamDao teamDao, IPlayerDao playerDao, ITournamentRegistrationDao registrationDao, ITournamentDao tournamentDao)
        {
            _teamDao = teamDao;
            _playerDao = playerDao;
            _registrationDao = registrationDao;
            _tournamentDao = tournamentDao;
        }

        public TeamResult tryCreateTeam(TeamRequest request)
        {
            var result = new TeamResult();
            var team = new TeamV2
            {
                TeamName = request.Name,
                TeamDivision = request.DivisionId
            };
            _teamDao.addTeam(team);
            result.success = true;
            result.team = team;
            return result;
        }

        public TeamResult tryUpdateTeam(TeamRequest request)
        {
            var result = new TeamResult();
            var team = new TeamV2
            {
                TeamId = request.Id,
                TeamName = request.Name,
                TeamDivision = request.DivisionId
            };
            _teamDao.editTeam(team);
            result.success = true;
            return result;
        }

        public TeamResult tryDeleteTeam(TeamRequest request)
        {
            var result = new TeamResult();
            var team = _teamDao.findTeam(new TeamV2 { TeamId = request.Id });
            if (team == null || team.TeamId == 0)
            {
                result.Errors.Add("Team not found.");
                return result;
            }
            _teamDao.removeTeam(team);
            result.success = true;
            return result;
        }

        public TeamResult tryGetTeam(TeamRequest request)
        {
            var result = new TeamResult();
            var team = _teamDao.findTeam(new TeamV2 { TeamId = request.Id });
            if (team == null || team.TeamId == 0)
            {
                result.Errors.Add("Team not found.");
                return result;
            }
            result.team = team;
            result.success = true;
            return result;
        }

        public PlayerResult tryAddPlayer(PlayerRequest request)
        {
            var result = new PlayerResult();
            var player = new PlayerV2
            {
                TeamId = request.TeamId,
                PlayerName = request.Name,
                Email = request.Email,
                City = request.City,
                Province = request.Province,
                Phone = request.Phone
            };
            _playerDao.addPlayer(player);
            result.success = true;
            return result;
        }

        public PlayerResult tryUpdatePlayer(PlayerRequest request)
        {
            var result = new PlayerResult();
            var player = new PlayerV2
            {
                PlayerId = request.Id,
                TeamId = request.TeamId,
                PlayerName = request.Name,
                Email = request.Email,
                City = request.City,
                Province = request.Province,
                Phone = request.Phone
            };
            _playerDao.editPlayer(player);
            result.success = true;
            return result;
        }

        public PlayerResult tryDeletePlayer(PlayerRequest request)
        {
            var result = new PlayerResult();
            var player = _playerDao.findPlayer(new PlayerV2 { PlayerId = request.Id });
            if (player == null || player.PlayerId == 0)
            {
                result.Errors.Add("Player not found.");
                return result;
            }
            _playerDao.removePlayer(player);
            result.success = true;
            return result;
        }

        public RegisterTeamResult tryMarkRegistrationPaid(RegisterTeamRequest request)
            //Note: Because id passed in by request is now the register's ID on the registration table instead of the 
            //  team table, we can just directly grab the registration now
        {
            var result = new RegisterTeamResult();
//            var team = _teamDao.findTeam(new TeamV2 { TeamId = request.TeamId });
            var registration = _registrationDao.findById(request.Id);
            if (registration == null || registration.TeamId == 0)
            {
                result.Errors.Add("Team not found.");
                return result;
            }
            registration.Status = RegistrationStatus.Paid;
            registration.StatusDate = DateTime.Now;
            _teamDao.saveChanges();
            result.success = true;
            return result;
        }

        public RegisterTeamResult tryRegisterTeam(RegisterTeamRequest request)
            //Doesn't stop unpaying teams from registering somehow?
            //Blocks double-registration but error message given is incorrect
            //  (Somehow giving "Paid registration" error?)
            //Prevents tournament exceeding capacity only sometimes somehow?
        {
            var result = new RegisterTeamResult();

            // Rule: team must have exactly 4 players
            // I'll need to set up unit testing for this later but should work looking at this)
            var players = _playerDao.findPlayersByTeam(new TeamV2 { TeamId = request.TeamId });
            if (players.Count != 4)
            {
                result.Errors.Add("A team must have exactly four players before registering.");
                return result;
            }

            // Rule: team must have paid
            var existingReg = _registrationDao.findRegistrationbyTeamAndTournament(request.TeamId, request.TournamentId);
            if (existingReg.RegistrationId != 0 && existingReg.Status != RegistrationStatus.Paid)
            {
                result.Errors.Add("A team must have paid the registration fee before registering.");
                return result;
            }

            // Rule: team cannot register twice
            if (existingReg.RegistrationId != 0)
            {
                result.Errors.Add("This team is already registered for this tournament.");
                return result;
            }

            // Rule: tournament cannot exceed capacity
            var tournament = _tournamentDao.findTournament(new Tournament { TournamentId = request.TournamentId });
            if (tournament.TournamentId == 0)
            {
                result.Errors.Add("Tournament not found.");
                return result;
            }
            var currentRegistrations = _registrationDao.getRegistrationsByTournament(request.TournamentId);
            if (currentRegistrations.Count > tournament.TeamCapacity)
            {
                result.Errors.Add("This tournament has reached its team capacity.");
                return result;
            }

            var registration = new Registration
            {
                TeamId = request.TeamId,
                TournamentId = request.TournamentId,
                RegisteredOn = DateTime.Now,
                Status = request.Status,
                StatusDate = DateTime.Now
            };
            _registrationDao.addRegistration(registration);
            result.success = true;
            return result;
        }
    }
}