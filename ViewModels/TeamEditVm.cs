using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace bowling_tournament_MVCPRoject.ViewModels
{
    public class TeamEditVm
    {
        public int? TeamId { get; set; }

        [Required]
        [StringLength(100)]
        public string TeamName { get; set; }

        [Required]
        public int DivisionId { get; set; }

        public bool RegistrationPaid { get; set; }

        public DateTime? PaymentDate { get; set; }

        public IEnumerable<SelectListItem> DivisionOptions { get; set; }

        public List<PlayerEditVm> NewPlayers { get; set; } = new();


    }
}
