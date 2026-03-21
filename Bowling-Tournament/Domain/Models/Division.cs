using System.ComponentModel.DataAnnotations;

namespace bowling_tournament_MVCPRoject.Domain.Models
{
    public class Division
    {
        public int DivisionId { get; set; }

        [Required]
        public string DivisionName { get; set; }

        public ICollection<Team> Teams { get; set; } = new List<Team>();
    }
}
