using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ReadModels;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                 where p.TeamId == team.id
                 select new PlayerListItem
                 {
                     PlayerId = p.PlayerId,
                     Name = p.PlayerName,
                     City = p.City,
                     Province = p.Province,
                     Email = p.Email,
                     Phone = p.Phone
                 }
                 ).ToListAsync();
        }

        public async Task<TeamListItem?> GetByIdAsync(int id)
        {
            var team = await _db.Team
                 .Where(t => t.TeamId == id)
                 .Select(t => new TeamListItem
                 {
                     id = t.TeamId,
                     teamName = t.TeamName,
                     teamDivision = t.TeamDivision
                 }
                ).FirstOrDefaultAsync();

            if (team == null) return null;

            team.Players = await GetTeamPlayersAsync(team);
            return team;
        }

        public async Task<List<SelectListItem>> GetDivisionOptionsAsync()
        {
            return await _db.Division
                .OrderBy(d => d.DivisionName)
                .Select(d => new SelectListItem
                {
                    Value = d.DivisionId.ToString(),
                    Text = d.DivisionName
                }).ToListAsync();
        }
    }
}
