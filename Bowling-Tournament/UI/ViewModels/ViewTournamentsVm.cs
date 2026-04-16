namespace bowling_tournament_MVCPRoject.UI.ViewModels
{
    public class ViewTournamentsVm
    {
        public List<TournamentWithCapacityVm> Tournaments { get; set; } = new();
    }

    public class TournamentWithCapacityVm
    {
        public int Id { get; set; }
        public string? TournamentName { get; set; }
        public DateTime TournamentDate { get; set; }
        public string? Location { get; set; }
        public int TeamCapacity { get; set; }
        public int WatcherCapacity { get; set; }
        public bool RegistrationOpen { get; set; }

        // Division limits
        public int MenCapacity { get; set; }
        public int WomenCapacity { get; set; }
        public int MixedCapacity { get; set; }
        public int YouthCapacity { get; set; }
        public int SeniorCapacity { get; set; }

        // Remaining capacities
        public int MenRemaining { get; set; }
        public int WomenRemaining { get; set; }
        public int MixedRemaining { get; set; }
        public int YouthRemaining { get; set; }
        public int SeniorRemaining { get; set; }

        // Overall team capacity
        public int TeamRemaining { get; set; }
    }
}
