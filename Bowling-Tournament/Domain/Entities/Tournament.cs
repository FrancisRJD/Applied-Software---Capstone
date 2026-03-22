namespace bowling_tournament_MVCPRoject.Domain.Entities
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
        public DateTime TournamentDate { get; set; }
        public string Location { get; set; }
        public int TeamCapacity { get; set; }
        public int WatcherCapacity { get; set; }
        public bool RegistrationOpen { get; set; }
    }
}
