using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.UI.ViewModels
{
    //Made a separate TeamVM intended for displaying teams/details only - Francis
    public class TeamDisplayVM
    {
        public int TeamId { get; set; } //Not normally displayed, just there for db fetching when grabbing a team's players
        public string TeamName { get; set; }
        public string TeamDivision { get; set; }

        public bool IsPaid { get; set; }
        public DateTime? DatePaid { get; set; }

        public IEnumerable<Player> Players { get; set; } //Null in some instances
    }
}
