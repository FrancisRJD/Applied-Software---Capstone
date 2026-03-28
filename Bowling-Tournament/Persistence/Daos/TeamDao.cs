using bowling_tournament_MVCPRoject.Domain.Daos;
using bowling_tournament_MVCPRoject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Persistence.Daos
{
    public class TeamDao : ITeamDao
    {
        private readonly BowlingDbContextV2 _db;

        public TeamDao(BowlingDbContextV2 db)
        {
            _db = db;
        }

        public void addTeam(TeamV2 team)
        {
            _db.Team.Add(team);
            _db.SaveChanges();
        }

        /// <summary>
        /// Updates team with new details. Doesn't update player details! Use PlayerDao.editPlayer() for this!
        /// </summary>
        /// <param name="team"></param>
        public void editTeam(TeamV2 team)
        {
            var teamFound = _db.Team.FirstOrDefault(t=>t.TeamId == team.TeamId);

            if (teamFound == null)
                return;

            teamFound.TeamName = team.TeamName;
            teamFound.TeamDivision = team.TeamDivision;

            _db.SaveChanges();
        }

        public TeamV2 findTeam(TeamV2 team)
        {
            return _db.Team.Find(team.TeamId) ?? new TeamV2();
        }

        /// <summary>
        /// Removes team. Players *should* be removed first (Though the database might be able to automatically handle this)
        /// </summary>
        /// <param name="team"></param>
        public void removeTeam(TeamV2 team)
        {
            _db.Team.Remove(team);
            _db.SaveChanges();
        }

        public void saveChanges()
        {
            _db.SaveChanges();
        }
    }
}
