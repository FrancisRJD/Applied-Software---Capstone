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

        public TournamentRequest(
            int id, 
            string name,
            int watcherCapacity,
            DateTime DateOfGame,
            string location,
            int teamCapacity,
            bool registrationOpen
            ) {
            this.Id = id;
            this.Name = name;
            this.WatcherCapacity = watcherCapacity;
            this.DateOfGame = DateOfGame;
            this.Location = location;
            this.TeamCapacity = teamCapacity;
            this.RegistrationOpen = registrationOpen;
        }
    }
}
