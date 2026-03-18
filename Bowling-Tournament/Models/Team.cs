using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.Models
{
    public class Team
    {
        public int TeamId { get; set; }

        [Required]
        public string TeamName { get; set; }

        [Required]
        public int DivisionId { get; set; }
        public Division Division { get; set; }

        [Required]
        public bool RegistrationPaid { get; set; }

        public DateTime? PaymentDate { get; set; }

        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
