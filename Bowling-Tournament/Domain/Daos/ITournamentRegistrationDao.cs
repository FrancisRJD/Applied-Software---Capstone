using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Daos
{
    public interface ITournamentRegistrationDao
    {
        /*
            Likely needed Dao methods
                READING registration by teamID, tournamentID
                READING registrations by tournamentID
                UPDATING registration status by teamID, tournamentID
                WRITING new registration
                DELETING registration by registrationID
         */

        public Registration? findById(int id);

        public Registration? findRegistrationbyTeamAndTournament(int teamId, int tournamentId);
        public List<Registration> getRegistrationsByTournament(int tournamentId);
        public void updateRegistration(Registration registration);
        public void addRegistration(Registration registration);
        public void removeRegistration(Registration registration);

        public void saveChanges();
    }
}
