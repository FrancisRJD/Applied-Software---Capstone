using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.UI.ViewModels
{
    public class TeamRegisterVm
    {
        public TeamRegisterVm()
        {
            Players = new List<PlayerVm>();
            for (int i = 0; i < 4; i++)
            {
                Players.Add(new PlayerVm());
            }
        }

        [Required]
        public string? TeamName { get; set; }

        [Required]
        public int DivisionId { get; set; }

        [MinLength(4)]
        [MaxLength(4)]
        public List<PlayerVm> Players { get; set; }

        public IEnumerable<SelectListItem>? DivisionOptions { get; set; }
    }
}
