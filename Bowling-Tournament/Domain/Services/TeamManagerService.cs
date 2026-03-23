using bowling_tournament_MVCPRoject.Domain.Daos;
using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
using bowling_tournament_MVCPRoject.Domain.Dtos.Results;
using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public class TeamManagerService : ITeamManagerService
    {
        private readonly ITeamDao _teamDao;
        private readonly IPlayerDao _playerDao;
        private readonly ITournamentDao _tournamentDao;
        private readonly ITournamentRegistrationDao _registrationDao;

        public TeamManagerService(
            ITeamDao teamDao,
            IPlayerDao playerDao,
            ITournamentDao tournamentDao,
            ITournamentRegistrationDao registrationDao)
        {
            _teamDao = teamDao;
            _playerDao = playerDao;
            _tournamentDao = tournamentDao;
            _registrationDao = registrationDao;
        }

        //Refer to ITeamManagerService for what I'd suggest the request objects to have
        //PLAYER-SPECIFIC
        public PlayerResult tryAddPlayer(PlayerRequest request)
        {
            PlayerResult result = validatePlayerRequest(request, false);

            if (!result.success)
                return result;

            TeamV2 team = _teamDao.findTeam(new TeamV2 { TeamId = request.TeamId });

            if (team == null || team.TeamId <= 0)
            {
                result.Errors.Add("Team not found.");
                result.success = false;
                return result;
            }

            List<PlayerV2> existingPlayers = _playerDao.findPlayersByTeam(new TeamV2 { TeamId = request.TeamId });

            if (existingPlayers.Count >= 4)
            {
                result.Errors.Add("Team already has four players.");
                result.success = false;
                return result;
            }

            PlayerV2 player = new PlayerV2
            {
                TeamId = request.TeamId,
                PlayerName = request.Name.Trim(),
                Email = request.Email.Trim(),
                City = request.City.Trim(),
                Province = request.Province.Trim(),
                Phone = string.Empty
            };

            _playerDao.addPlayer(player);

            result.success = true;
            return result;
        }

        public PlayerResult tryDeletePlayer(PlayerRequest request)
        {
            PlayerResult result = new PlayerResult();

            if (request == null)
            {
                result.Errors.Add("Request cannot be null.");
                return result;
            }

            if (request.Id <= 0)
            {
                result.Errors.Add("A valid player ID is required.");
                return result;
            }

            PlayerV2 player = _playerDao.findPlayer(new PlayerV2 { PlayerId = request.Id });

            if (player == null || player.PlayerId <= 0)
            {
                result.Errors.Add("Player not found.");
                return result;
            }

            _playerDao.removePlayer(player);

            result.success = true;
            return result;
        }

        //TEAM-SPECIFIC
        public TeamResult tryCreateTeam(TeamRequest request)
        {
            TeamResult result = validateTeamRequest(request, false);

            if (!result.success)
            {
                return result;
            }

            TeamV2 team = new TeamV2
            {
                TeamName = request.Name.Trim(),
                TeamDivision = request.DivisionId
            };

            _teamDao.addTeam(team);

            result.success = true;
            result.teamV2 = team;
            return result;
        }

        public TeamResult tryUpdateTeam(TeamRequest request)
        {
            TeamResult result = validateTeamRequest(request, true);

            if (!result.success)
            {
                return result;
            }

            TeamV2 existingTeam = _teamDao.findTeam(new TeamV2 { TeamId = request.Id });

            if (existingTeam == null || existingTeam.TeamId <= 0)
            {
                result.Errors.Add("Team not found.");
                result.success = false;
                return result;
            }

            existingTeam.TeamName = request.Name.Trim();
            existingTeam.TeamDivision = request.DivisionId;

            _teamDao.editTeam(existingTeam);

            result.success = true;
            result.teamV2 = existingTeam;
            return result;
        }

        public TeamResult tryDeleteTeam(TeamRequest request)
        {
            TeamResult result = new TeamResult();

            if (request == null)
            {
                result.Errors.Add("Request cannot be null.");
                return result;
            }

            if (request.Id <= 0)
            {
                result.Errors.Add("A valid team ID is required.");
                return result;
            }

            TeamV2 existingTeam = _teamDao.findTeam(new TeamV2 { TeamId = request.Id });

            if (existingTeam == null || existingTeam.TeamId <= 0)
            {
                result.Errors.Add("Team not found.");
                return result;
            }

            _teamDao.removeTeam(existingTeam);
            _teamDao.saveChanges();

            result.success = true;
            result.teamV2 = existingTeam;
            return result;
        }

        public TeamResult tryGetTeam(TeamRequest request)
        {
            TeamResult result = new TeamResult();

            if (request == null)
            {
                result.Errors.Add("Request cannot be null.");
                return result;
            }

            if (request.Id <= 0)
            {
                result.Errors.Add("A valid team ID is required.");
                return result;
            }

            TeamV2 existingTeam = _teamDao.findTeam(new TeamV2 { TeamId = request.Id });

            if (existingTeam == null || existingTeam.TeamId <= 0)
            {
                result.Errors.Add("Team not found.");
                return result;
            }

            result.success = true;
            result.teamV2 = existingTeam;
            return result;
        }

        public RegisterTeamResult tryMarkTeamPaid(RegisterTeamRequest request)
        {
            RegisterTeamResult result = validateRegisterRequest(request);

            if (!result.success)
                return result;

            TeamV2 team = _teamDao.findTeam(new TeamV2 { TeamId = request.TeamId });
            if (team == null || team.TeamId <= 0)
            {
                result.Errors.Add("Team not found.");
                result.success = false;
                return result;
            }

            Tournament tournament = _tournamentDao.findTournament(new Tournament { TournamentId = request.TournamentId });
            if (tournament == null || tournament.TournamentId <= 0)
            {
                result.Errors.Add("Tournament not found.");
                result.success = false;
                return result;
            }

            Registration registration = _registrationDao.findRegistrationbyTeamAndTournament(request.TeamId, request.TournamentId);

            if (registration == null || registration.RegistrationId <= 0)
            {
                result.Errors.Add("Registration not found.");
                result.success = false;
                return result;
            }

            registration.Status = RegistrationStatus.Paid;
            registration.StatusDate = DateTime.Now;

            _registrationDao.updateRegistration(registration);

            result.success = true;
            return result;
        }

        public RegisterTeamResult tryRegisterTeam(RegisterTeamRequest request)
        {
            RegisterTeamResult result = validateRegisterRequest(request);

            if (!result.success)
                return result;

            TeamV2 team = _teamDao.findTeam(new TeamV2 { TeamId = request.TeamId });
            if (team == null || team.TeamId <= 0)
            {
                result.Errors.Add("Team not found.");
                result.success = false;
                return result;
            }

            Tournament tournament = _tournamentDao.findTournament(new Tournament { TournamentId = request.TournamentId });
            if (tournament == null || tournament.TournamentId <= 0)
            {
                result.Errors.Add("Tournament not found.");
                result.success = false;
                return result;
            }

            if (!tournament.RegistrationOpen)
            {
                result.Errors.Add("Tournament registration is closed.");
                result.success = false;
                return result;
            }

            List<PlayerV2> players = _playerDao.findPlayersByTeam(new TeamV2 { TeamId = request.TeamId });
            if (players.Count != 4)
            {
                result.Errors.Add("A team must have exactly four players before registering.");
                result.success = false;
                return result;
            }

            Registration existingRegistration = _registrationDao.findRegistrationbyTeamAndTournament(request.TeamId, request.TournamentId);
            if (existingRegistration != null && existingRegistration.RegistrationId > 0)
            {
                result.Errors.Add("Team is already registered for this tournament.");
                result.success = false;
                return result;
            }

            List<Registration> registrations = _registrationDao.getRegistrationsByTournament(request.TournamentId);
            if (registrations.Count >= tournament.TeamCapacity)
            {
                result.Errors.Add("Tournament is already full.");
                result.success = false;
                return result;
            }

            Registration registration = new Registration
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


        private TeamResult validateTeamRequest(TeamRequest request, bool isUpdate)
        {
            TeamResult result = new TeamResult();

            if (request == null)
            {
                result.Errors.Add("Request cannot be null.");
            }
            else
            {
                if (isUpdate && request.Id <= 0)
                {
                    result.Errors.Add("A valid team ID is required for update.");
                }

                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    result.Errors.Add("Team name is required.");
                }

                if (request.DivisionId <= 0)
                {
                    result.Errors.Add("A valid division ID is required.");
                }
            }

            result.success = result.Errors.Count == 0;
            return result;
        }


        private PlayerResult validatePlayerRequest(PlayerRequest request, bool isUpdate)
        {
            PlayerResult result = new PlayerResult();

            if (request == null)
            {
                result.Errors.Add("Request cannot be null.");
            }
            else
            {
                if (isUpdate && request.Id <= 0)
                    result.Errors.Add("A valid player ID is required.");

                if (request.TeamId <= 0)
                    result.Errors.Add("A valid team ID is required.");

                if (string.IsNullOrWhiteSpace(request.Name))
                    result.Errors.Add("Player name is required.");

                if (string.IsNullOrWhiteSpace(request.Email))
                    result.Errors.Add("Player email is required.");

                if (string.IsNullOrWhiteSpace(request.City))
                    result.Errors.Add("City is required.");

                if (string.IsNullOrWhiteSpace(request.Province))
                    result.Errors.Add("Province is required.");
            }

            result.success = result.Errors.Count == 0;
            return result;
        }

        private RegisterTeamResult validateRegisterRequest(RegisterTeamRequest request)
        {
            RegisterTeamResult result = new RegisterTeamResult();

            if (request == null)
            {
                result.Errors.Add("Request cannot be null.");
            }
            else
            {
                if (request.TeamId <= 0)
                    result.Errors.Add("A valid team ID is required.");

                if (request.TournamentId <= 0)
                    result.Errors.Add("A valid tournament ID is required.");
            }

            result.success = result.Errors.Count == 0;
            return result;
        }
    }
}
