using bowling_tournament_MVCPRoject.UI.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bowling_tournament_MVCPRoject.UI.ViewModels
{
    public class TeamCreateVm
    {
        public string? TeamName { get; set; }

        public int DivisionId { get; set; }

        public List<PlayerEditVm> NewPlayers { get; set; } = new();

        public IEnumerable<SelectListItem>? DivisionOptions { get; set; }
        public IEnumerable<SelectListItem>? PlayerOptions { get; set; }
    }
}