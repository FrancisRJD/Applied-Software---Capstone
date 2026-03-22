namespace bowling_tournament_MVCPRoject.Domain.Entities
{
    public class Registration
    {
        public int RegistrationId { get; set; }
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        public DateTime RegisteredOn { get; set; }
        public RegistrationStatus Status { get; set; }
        public DateTime StatusDate { get; set; }
    }
}
