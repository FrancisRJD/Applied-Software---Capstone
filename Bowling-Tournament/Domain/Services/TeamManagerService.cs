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

        public RegisterTeamResult tryMarkPaid(RegisterTeamRequest request)
            //Note to self: Because registrations per specs are on a PER TEAM basis you actually need to mark the team as paid separately!
            //I need to rename this to "tryMarkTeamPaid" so this is clear...
        {
            var result = new RegisterTeamResult();
            var team = _teamDao.findTeam(new TeamV2 { TeamId = request.TeamId });
//            var registration = _registrationDao.findById(request.Id);
            if (team == null || team.TeamId == -1)
            {
                result.Errors.Add("Team not found.");
                return result;
            }

            team.RegistrationPaid = true;
            team.PaymentDate = DateTime.Now;
            _teamDao.saveChanges();
            result.success = true;
            return result;
        }

        public RegisterTeamResult tryRegisterTeam(RegisterTeamRequest request)
            //Doesn't stop unpaying teams from registering somehow? [FIXED]
            //Blocks double-registration but error message given is incorrect [FIXED-ISH]
            //Prevents tournament exceeding capacity only sometimes somehow?
        {
            var result = new RegisterTeamResult();

            // Rule: team must have exactly 4 players
            var players = _playerDao.findPlayersByTeam(new TeamV2 { TeamId = request.TeamId });
            bool playerDetailsNotFilled = false;

            //I figured out the issue with the players-- when registering a team the form does not
            //  properly check the player's details (Checks min/max length of player object, but NOT
                // what the player object's details are filled with!!!)
            //As a result, the players DO exist but with empty details in this context. My temp fix is
            //  to just check if any of the players have an empty name (As name is required).
            //      - Francis

            foreach (var player in players){
                if (player.PlayerName.Trim() == "") playerDetailsNotFilled = true;
            }

            if (players.Count != 4 || playerDetailsNotFilled)
            {
                result.Errors.Add("A team must have exactly four players before registering.");
                return result;
            }

            // Rule: team must have paid
            var existingTeam = _teamDao.findTeam(new TeamV2 { TeamId = request.TeamId });
            if (!existingTeam.RegistrationPaid)
            {
                result.Errors.Add("A team must have paid the registration fee before registering.");
                return result;
            }

            var existingReg = _registrationDao.findRegistrationbyTeamAndTournament(request.TeamId, request.TournamentId);
            // Rule: team cannot register twice
            if (existingReg != null)
            {
                result.Errors.Add("This team is already registered for this tournament.");
                return result;
            }

            // Rule: tournament cannot exceed capacity
            var tournament = _tournamentDao.findTournament(new Tournament { TournamentId = request.TournamentId });
            if (tournament.TournamentId == -1) //Set up findTournament to return a tournament with id -1 for empty returns
            {
                result.Errors.Add("Tournament not found.");
                return result;
            }

            //Rule: Tournament registration must be open
            if(!tournament.RegistrationOpen)
            {
                result.Errors.Add("Tournament is closed for registrations");
                return result;
            }

            var currentRegistrations = _registrationDao.getRegistrationsByTournament(request.TournamentId);
            var registrationStatus = currentRegistrations.Count >= tournament.TeamCapacity ? RegistrationStatus.Waitlisted : RegistrationStatus.Registered;

            var registration = new Registration
            {
                TeamId = request.TeamId,
                TournamentId = request.TournamentId,
                RegisteredOn = DateTime.Now,
                Status = registrationStatus,
                StatusDate = DateTime.Now
            };
            _registrationDao.addRegistration(registration);
            result.success = true;
            return result;
        }
    }
}