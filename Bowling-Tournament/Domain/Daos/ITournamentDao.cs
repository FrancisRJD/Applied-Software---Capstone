using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Daos
{
    public interface ITournamentDao
    {
        /*
            Likely needed Dao methods
                READING tournament by tournamentID
                WRITING new tournament
                DELETING tournament by tournamentID
         */

        public Tournament findTournament(Tournament tournament);
        public void addTournament (Tournament tournament);
        public void removeTournament (Tournament tournament);
        public void saveChanges();
    }
}
