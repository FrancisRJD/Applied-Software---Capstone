using bowling_tournament_MVCPRoject.Domain.Daos;
using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Persistence.Daos
{
    public class TournamentRegistrationDao : ITournamentRegistrationDao
    {
        public void addRegistration(Registration registration)
        {
            throw new NotImplementedException();
        }

        public Registration findRegistrationbyTeamAndTournament(int teamId, int tournamentId)
        {
            throw new NotImplementedException();
        }

        public List<Registration> getRegistrationsByTournament(int tournamentId)
        {
            throw new NotImplementedException();
        }

        public void removeRegistration(Registration registration)
        {
            throw new NotImplementedException();
        }

        public void saveChanges()
        {
            throw new NotImplementedException();
        }

        public void updateRegistration(Registration registration)
        {
            throw new NotImplementedException();
        }
    }
}
