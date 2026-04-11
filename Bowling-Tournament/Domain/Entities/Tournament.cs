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
        
        //HARD-CODED DIVISION CAPS (I decided to hard-code this as this is the last sprint with major changes and I want to *hopefully* make the new
        //  features as simple as possible to implement in exchange for the tech debt) - Francis
            // -1 is meant to represent a capacity with no limit (There can be as many mens' division teams up to the TOTAL team capacity)
        public int MensCapacity { get; set; } = -1;
        public int WomensCapacity { get; set; } = -1;
        public int MixedCapacity { get; set; } = -1;
        public int YouthCapacity { get; set; } = -1;
        public int SeniorCapacity { get; set; } = -1;

    }
}
