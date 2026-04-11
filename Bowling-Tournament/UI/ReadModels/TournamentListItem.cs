namespace bowling_tournament_MVCPRoject.UI.ReadModels
{
    public class TournamentListItem
    {
        public int id {  get; set; }
        public string? tournamentName { get; set; }
        public DateTime tournamentDate  { get; set; }
        public string? location { get; set; }
        public int teamCapacity { get; set; }
        public int watcherCapacity { get; set; }
        public bool registrationOpen { get; set; }

        public int menCapacity { get; set; }
        public int womenCapacity { get; set; }
        public int mixedCapacity { get; set; }
        public int youthCapacity { get; set; }
        public int seniorCapacity { get; set; }
    }
}
