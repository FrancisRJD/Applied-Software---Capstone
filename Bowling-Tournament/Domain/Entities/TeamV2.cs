namespace bowling_tournament_MVCPRoject.Domain.Entities
{
    public class TeamV2
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = "";
        public int TeamDivision { get; set; }
        public bool RegistrationPaid { get; set; } //Set up to no longer be a bool so registrations can be moved to hold or active based on tournament capacity
        public DateTime? PaymentDate { get; set; }
    }
}
