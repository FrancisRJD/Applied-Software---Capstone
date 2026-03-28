using bowling_tournament_MVCPRoject.UI.ReadModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bowling_tournament_MVCPRoject.UI.Queries
{
    public interface ITeamReadModelGateway
    //Gimme a quick heads up if you can if there's other read-only data you need from persistence! - Francis
    //Also set this up to be the one responsible for grabbing a team's players. Since they don't exist separate from a team anyways.
    {
        Task<List<TeamListItem>> GetAllAsync();
        Task<List<TeamListItem>> GetAllInTournamentAsync(TournamentListItem tournament);
        Task<List<PlayerListItem>> GetTeamPlayersAsync(TeamListItem team);
        Task<TeamListItem?> GetByIdAsync(int id);
        Task<List<SelectListItem>> GetDivisionOptionsAsync();
        Task<List<RegistrationListItem>> GetAllTeamRegistrations();
    }
}
