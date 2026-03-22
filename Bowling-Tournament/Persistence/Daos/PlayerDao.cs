using bowling_tournament_MVCPRoject.Domain.Daos;
using bowling_tournament_MVCPRoject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Persistence.Daos
{
    public class PlayerDao : IPlayerDao
    {
        private readonly BowlingDbContextV2 _db;

        public PlayerDao(BowlingDbContextV2 db)
        {
            _db = db;
        }

        public void addPlayer(PlayerV2 player)
        {
            _db.Player.Add(player);
            _db.SaveChanges();
        }

        public void editPlayer(PlayerV2 player)
        {
            var playerFound = _db.Player.FirstOrDefault(p=> p.PlayerId == player.PlayerId);

            if (playerFound == null)
                return;

            playerFound.PlayerName = player.PlayerName;
            playerFound.Phone = player.Phone;
            playerFound.Email = player.Email;
            playerFound.City = player.City;
            playerFound.Province = player.Province;

            _db.SaveChanges();
        }

        public PlayerV2 findPlayer(PlayerV2 player)
        {
            return _db.Player.Find(player.PlayerId) ?? new PlayerV2();
        }

        public PlayerV2 findPlayerByTeam(TeamV2 team)
        {
            return _db.Player
                .Where(x => x.TeamId == team.TeamId)
                .FirstOrDefault() ?? new PlayerV2();
        }

        public void removePlayer(PlayerV2 player)
        {
            _db.Player.Remove(player);
            _db.SaveChanges();
        }

        public void saveChanges()
        {
            _db.SaveChanges();
        }
    }
}
