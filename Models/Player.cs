using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.Models
{
    public class Player
    {
        public int PlayerId { get; set; }

        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; }

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
        public string Phone { get; set; }
    }
}
