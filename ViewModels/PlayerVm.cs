using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.ViewModels
{
    public class PlayerVm
    {
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

        public TeamCreateVm teamVmAttachment { get; set; }

    }
}
