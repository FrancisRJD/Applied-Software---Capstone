using bowling_tournament_MVCPRoject.UI.ReadModels;
using bowling_tournament_MVCPRoject.UI.ViewModels;

namespace bowling_tournament_MVCPRoject.UI.Queries
{
    public interface ITournamentReadModelGateway
        //Gimme a quick heads up if you can if there's other read-only data you need from persistence! - Francis
    {
        Task<List<TournamentListItem>> GetAllAsync();
        Task<List<TournamentOptions>> GetTournamentOptionsAsync();
        Task<TournamentListItem?> GetByIdAsync(int id);
        Task<List<TournamentWithCapacityVm>> GetAllWithCapacityAsync();
    }
}
