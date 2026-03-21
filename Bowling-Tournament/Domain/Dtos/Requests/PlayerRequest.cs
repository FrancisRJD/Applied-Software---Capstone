namespace bowling_tournament_MVCPRoject.Domain.Dtos.Requests
{
    public class PlayerRequest
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public PlayerRequest(
            int playerId,
            int teamId,
            string name,
            string email,
            string city,
            string province
            ) { 
            Id = playerId;
            TeamId = teamId;
            Name = name;
            Email = email;
            City = city;
            Province = province;
        }
    }
}
