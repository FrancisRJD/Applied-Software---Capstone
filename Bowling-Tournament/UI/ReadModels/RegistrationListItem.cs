using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.UI.ReadModels
{
    public class RegistrationListItem
    {
        public int id { get; set; }
        public TournamentListItem tournament { get; set; }
        public TeamListItem team { get; set; }
        public DateTime registeredOn { get; set; }
        public RegistrationStatus registrationStatus { get; set; }
        public DateTime statusDate { get; set; }
    }
}
