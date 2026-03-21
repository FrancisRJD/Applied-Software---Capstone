using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
using bowling_tournament_MVCPRoject.Domain.Dtos.Results;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public class TeamManagerService : ITeamManagerService
    {
        //Refer to ITeamManagerService for what I'd suggest the request objects to have
        //PLAYER-SPECIFIC
        public PlayerResult tryAddPlayer(PlayerRequest request)
        {
            throw new NotImplementedException();
        }

        public PlayerResult tryDeletePlayer(PlayerRequest request)
        {
            throw new NotImplementedException();
        }

        //TEAM-SPECIFIC
        public TeamResult tryCreateTeam(TeamRequest request)
        {
            throw new NotImplementedException();
        }


        public TeamResult tryDeleteTeam(TeamRequest request)
        {
            throw new NotImplementedException();
        }

        public TeamResult tryGetTeam(TeamRequest request)
        {
            throw new NotImplementedException();
        }

        public RegisterTeamResult tryMarkTeamPaid(RegisterTeamRequest request)
        {
            throw new NotImplementedException();
        }

        public RegisterTeamResult tryRegisterTeam(RegisterTeamRequest request)
            //Reminder to self incase it's me handling domain here, this method is for registering an existing team to a tournament!
        {
            throw new NotImplementedException();
        }

        public TeamResult tryUpdateTeam(TeamRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
