using bowling_tournament_MVCPRoject.Domain.Dtos.Requests;
using bowling_tournament_MVCPRoject.Domain.Dtos.Results;
using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Services
{
    public interface ITeamManagerService
    {
        //TEAM-SPECIFIC
        RegisterTeamResult tryRegisterTeam(RegisterTeamRequest request);
        //Request contains registration details (TeamID, tournamentID)

        // Cancel
        RegisterTeamResult tryCancelRegistration(RegisterTeamRequest request);

        TeamResult tryCreateTeam(TeamRequest request); 
            //Request contains ALL team details and 4 players. Does not contain a teamID (Since team will be assigned one)

        TeamResult tryUpdateTeam(TeamRequest request); 
            //Request contains updated team details with teamID (Players updated via team edit menu or player edit?)
            //Player doesn't exist outside of teams(And there's no player list view) so probably team edit?
        TeamResult tryDeleteTeam(TeamRequest request);
            //Request contains team ID, all other details null/duds. Players attached to team *then* team will be deleted.

        TeamResult tryGetTeam(TeamRequest request); 
            //Request contains team ID, all other details null/duds.
    
        //PLAYER-SPECIFIC
        PlayerResult tryAddPlayer(PlayerRequest request);
            //Request contains player details (Must also have teamID),
        PlayerResult tryDeletePlayer(PlayerRequest request);
            //Request contains playerID
        PlayerResult tryUpdatePlayer(PlayerRequest request);
        RegisterTeamResult tryMarkPaid(RegisterTeamRequest request);
    }
}
