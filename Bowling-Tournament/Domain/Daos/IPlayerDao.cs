using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Domain.Daos
{
    public interface IPlayerDao
    {
        public PlayerV2 findPlayer(PlayerV2 player);
        public PlayerV2 findPlayerByTeam(TeamV2 team);
        public void addPlayer(PlayerV2 player);
        public void editPlayer(PlayerV2 player);
        public void removePlayer(PlayerV2 player);

        public List<PlayerV2> findPlayersByTeam(TeamV2 team);
        public void saveChanges();
    }
}
