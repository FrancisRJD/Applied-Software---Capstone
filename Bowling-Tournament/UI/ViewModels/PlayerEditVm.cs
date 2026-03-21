using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.UI.ViewModels
{
    public class PlayerEditVm
    {
        public int? PlayerId { get; set; }

        [Required]
        public int TeamId { get; set; }

        [Required]
        public string PlayerName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
    }
}
