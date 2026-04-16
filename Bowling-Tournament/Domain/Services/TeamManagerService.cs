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
            if (existingReg != null && existingReg.RegistrationId != 0)
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
            var registeredRegistrations = currentRegistrations
                .Where(r => r.Status == RegistrationStatus.Registered)
                .ToList();

            int overallRegisteredCount = registeredRegistrations.Count;
            int sameDivisionRegisteredCount = getRegisteredCountForDivision(registeredRegistrations, existingTeam.TeamDivision);
            int divisionCapacity = getDivisionCapacity(tournament, existingTeam.TeamDivision);

            bool overallHasRoom = overallRegisteredCount < tournament.TeamCapacity;
            bool divisionHasRoom = divisionCapacity == -1 || sameDivisionRegisteredCount < divisionCapacity;

            var registrationStatus =
                (overallHasRoom && divisionHasRoom)
                    ? RegistrationStatus.Registered
                    : RegistrationStatus.Waitlisted;

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

        // Cancel registration
        public RegisterTeamResult tryCancelRegistration(RegisterTeamRequest request)
        {
            var result = new RegisterTeamResult();

            var registration = _registrationDao.findRegistrationbyTeamAndTournament(
                request.TeamId,
                request.TournamentId
            );

            if (registration == null || registration.RegistrationId == 0)
            {
                result.Errors.Add("Registration not found.");
                return result;
            }

            bool cancelledWasRegistered = registration.Status == RegistrationStatus.Registered;

            var cancelledTeam = _teamDao.findTeam(new TeamV2 { TeamId = registration.TeamId });
            int cancelledDivisionId = cancelledTeam != null ? cancelledTeam.TeamDivision : -1;
            var tournament = _tournamentDao.findTournament(new Tournament { TournamentId = request.TournamentId });

            var waitlist = _registrationDao.getRegistrationsByTournamentAndStatus(request.TournamentId, RegistrationStatus.Waitlisted);

            waitlist.Sort((a, b) => a.RegisteredOn.CompareTo(b.RegisteredOn));
            //Sort waitlist from oldest waitlist to youngest

            Registration pendingRegistration = null;

            if (cancelledWasRegistered)
            {
                var allRegistrations = _registrationDao.getRegistrationsByTournament(request.TournamentId);
                var registeredRegistrations = allRegistrations
                    .Where(r => r.Status == RegistrationStatus.Registered && r.RegistrationId != registration.RegistrationId)
                    .ToList();

                foreach (var waitlistedRegistration in waitlist)
                {
                    var waitlistedTeam = _teamDao.findTeam(new TeamV2 { TeamId = waitlistedRegistration.TeamId });

                    if (waitlistedTeam.TeamDivision != cancelledDivisionId)
                        continue;

                    int divisionCapacity = getDivisionCapacity(tournament, waitlistedTeam.TeamDivision);
                    int currentDivisionRegistered = getRegisteredCountForDivision(registeredRegistrations, waitlistedTeam.TeamDivision);

                    bool divisionHasRoom = divisionCapacity == -1 || currentDivisionRegistered < divisionCapacity;

                    if (divisionHasRoom)
                    {
                        pendingRegistration = waitlistedRegistration;
                        break;
                    }
                }

                // If none from same division, take oldest waitlisted from any division that has room
                if (pendingRegistration == null)
                {
                    foreach (var waitlistedRegistration in waitlist)
                    {
                        var waitlistedTeam = _teamDao.findTeam(new TeamV2 { TeamId = waitlistedRegistration.TeamId });

                        int divisionCapacity = getDivisionCapacity(tournament, waitlistedTeam.TeamDivision);
                        int currentDivisionRegistered = getRegisteredCountForDivision(registeredRegistrations, waitlistedTeam.TeamDivision);

                        bool divisionHasRoom = divisionCapacity == -1 || currentDivisionRegistered < divisionCapacity;

                        if (divisionHasRoom)
                        {
                            pendingRegistration = waitlistedRegistration;
                            break;
                        }
                    }
                }

                if (pendingRegistration != null)
                {
                    pendingRegistration.Status = RegistrationStatus.Registered;
                    pendingRegistration.StatusDate = DateTime.Now;
                    _registrationDao.updateRegistration(pendingRegistration);
                }
            }

            //if (registration == null || registration.RegistrationId == 0)
            //{
            //    result.Errors.Add("Registration not found.");
            //    return result;
            //}

            _registrationDao.removeRegistration(registration);
            result.success = true;
            return result;
        }

        private int getRegisteredCountForDivision(List<Registration> registeredRegistrations, int divisionId)
        {
            int count = 0;

            foreach (var registration in registeredRegistrations)
            {
                var team = _teamDao.findTeam(new TeamV2 { TeamId = registration.TeamId });

                if (team != null && team.TeamId != 0 && team.TeamDivision == divisionId)
                {
                    count++;
                }
            }

            return count;
        }

        private int getDivisionCapacity(Tournament tournament, int divisionId)
        {
            switch (divisionId)
            {
                case 1:
                    return tournament.MensCapacity;
                case 2:
                    return tournament.WomensCapacity;
                case 3:
                    return tournament.MixedCapacity;
                case 4:
                    return tournament.YouthCapacity;
                case 5:
                    return tournament.SeniorCapacity;
                default:
                    return -1;
            }
        }
    }
}