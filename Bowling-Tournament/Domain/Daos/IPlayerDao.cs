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
        public PlayerV2 findPlayer(PlayerV2 player); //Only uses playerId, other details ignored
        public PlayerV2 findPlayerByTeam(TeamV2 team); //Only uses teamId, other details ignored
        public void addPlayer(PlayerV2 player);
        public void editPlayer(PlayerV2 player);
        public void removePlayer(PlayerV2 player); //Only uses playerId, other details ignored

        public List<PlayerV2> findPlayersByTeam(TeamV2 team);
        public void saveChanges();
    }
}
