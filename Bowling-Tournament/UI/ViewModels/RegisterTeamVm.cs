using Microsoft.AspNetCore.Mvc.Rendering;

namespace bowling_tournament_MVCPRoject.UI.ViewModels
{
    public class RegisterTeamVm
    {
        public int TeamId { get; set; }
        public int TournamentId { get; set; }
        public IEnumerable<SelectListItem>? TournamentOptions { get; set; }
    }
}