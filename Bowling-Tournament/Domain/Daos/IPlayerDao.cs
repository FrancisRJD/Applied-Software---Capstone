using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Daos
{
    public interface IPlayerDao
    {
        /*
         * 
            Likely needed Dao methods
                READING player by playerID
                READING player(s) by teamID
                WRITING new player
                DELETING player by playerID
         */
        public Player findPlayer(Player player); //Only uses playerId, other details ignored
        public Player findPlayerByTeam(Team team); //Only uses teamId, other details ignored
        public void addPlayer(Player player);
        public void removePlayer(Player player); //Only uses playerId, other details ignored

        public void saveChanges();
    }
}
