using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Persistence.Queries
{
    public class TeamReadModelGateway : ITeamReadModelGateway
    {
        private readonly BowlingDbContextV2 _db;
        public TeamReadModelGateway(BowlingDbContextV2 context)
        {
            _db = context;
        }
        public async Task<List<TeamListItem>> GetAllAsync()
        {
            return await 
                (from t in _db.Team
                select new TeamListItem
                {
                    id = t.TeamId,
                    teamName = t.TeamName,
                    teamDivision = t.TeamDivision
                }
                ).ToListAsync();
        }

        public async Task<List<TeamListItem>> GetAllInTournamentAsync(TournamentListItem tournament)
        {
            return await
                (from t in _db.Team
                 join r in _db.Registration on t.TeamId equals r.TeamId
                 where r.TournamentId == tournament.id
                 select new TeamListItem
                 {
                     id = t.TeamId,
                     teamName = t.TeamName,
                     teamDivision = t.TeamDivision
                 }
                ).ToListAsync();
        }

        public async Task<List<PlayerListItem>> GetTeamPlayersAsync(TeamListItem team)
        {
            return await
                (from p in _db.Player
                 join t in _db.Team on p.TeamId equals p.TeamId
                 where p.TeamId == team.id
                 select new PlayerListItem
                 {
                     PlayerId = t.PlayerId,
                     Name = t.Name,
                     City = t.City,
                     Province = t.Province,
                     Email = t.Email,
                     Phone = t.Phone
                 }
                 ).ToListAsync();
        }
    }
}
