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
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Unpaid;
        public DateTime StatusDate { get; set; } = DateTime.Now;

        public RegisterTeamRequest(
                int registrationId,
                int teamId,
                int tournamentId,
                RegistrationStatus status
            ) { 
            Status = status;
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
