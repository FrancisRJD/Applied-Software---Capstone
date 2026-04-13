namespace bowling_tournament_MVCPRoject.Domain.Dtos.Requests
{
    public class TournamentRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WatcherCapacity { get; set; }
        public DateTime DateOfGame { get; set; }
        public string Location { get; set; }
        public int TeamCapacity { get; set; }
        public bool RegistrationOpen { get; set; }
        public int MensCapacity { get; set; } = -1;
        public int WomensCapacity { get; set; } = -1;
        public int MixedCapacity { get; set; } = -1;
        public int YouthCapacity { get; set; } = -1;
        public int SeniorCapacity { get; set; } = -1;

        public TournamentRequest(
            int id, 
            string name,
            int watcherCapacity,
            DateTime DateOfGame,
            string location,
            int teamCapacity,
            bool registrationOpen,
            int mensCapacity,
            int womensCapacity,
            int mixedCapacity,
            int youthCapacity,
            int seniorCapacity
            ) {
            this.Id = id;
            this.Name = name;
            this.WatcherCapacity = watcherCapacity;
            this.DateOfGame = DateOfGame;
            this.Location = location;
            this.TeamCapacity = teamCapacity;
            this.RegistrationOpen = registrationOpen;
            this.MensCapacity = mensCapacity;
            this.WomensCapacity = womensCapacity;
            this.MixedCapacity = mixedCapacity;
            this.YouthCapacity = youthCapacity;
            this.SeniorCapacity = seniorCapacity;
        }
    }
}
