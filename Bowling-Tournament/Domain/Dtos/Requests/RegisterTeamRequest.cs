using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Dtos.Requests
{
    public class RegisterTeamRequest
        //For registrations only!
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int TournamentId { get; set; }
        public DateTime RegisteredOn { get; set; } = DateTime.Now;
        public bool IsPaid { get; set; } = false;
        public RegistrationStatus RegisterStatus { get; set; } = RegistrationStatus.Registered;
        public DateTime StatusDate { get; set; } = DateTime.Now;

        public RegisterTeamRequest(
                int registrationId,
                int teamId,
                int tournamentId,
                bool isPaid
            ) {
            IsPaid = isPaid;
            TournamentId = tournamentId;
            TeamId = teamId;
            Id = registrationId;
        }
        public RegisterTeamRequest(
                int registrationId,
                int teamId,
                int tournamentId
            ) { 
            Id= registrationId;
            TeamId= teamId;
            TournamentId= tournamentId;
        }
    }
}
