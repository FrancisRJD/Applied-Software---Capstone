using bowling_tournament_MVCPRoject.Domain.Entities;
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
                 join d in _db.Division on t.TeamDivision equals d.DivisionId
                 select new TeamListItem
                 {
                     id = t.TeamId,
                     teamName = t.TeamName,
                     teamDivision = t.TeamDivision,
                     divisionName = d.DivisionName,
                     isPaid = t.RegistrationPaid,
                     paymentDate = t.PaymentDate
                 }
                ).ToListAsync();
        }

        public async Task<List<RegistrationListItem>> GetAllTeamRegistrations()
            //GetAllAsync is now for tournaments, this is specifically for *Registrations*
        {
            return await
                (from r in _db.Registration
                 join t in _db.Team on r.TeamId equals t.TeamId
                 join d in _db.Division on t.TeamDivision equals d.DivisionId
                 join to in _db.Tournament on r.TournamentId equals to.TournamentId
                 select new RegistrationListItem
                 {
                     id = r.RegistrationId,
                     tournament = new TournamentListItem
                     {
                         id = to.TournamentId,
                         tournamentName = to.TournamentName,
                         tournamentDate = to.TournamentDate,
                         location = to.Location,
                         teamCapacity = to.TeamCapacity,
                         watcherCapacity = to.WatcherCapacity,
                         registrationOpen = to.RegistrationOpen,
                     },
                     team = new TeamListItem
                     {
                         id = t.TeamId,
                         teamName = t.TeamName,
                         teamDivision = t.TeamDivision,
                         divisionName = d.DivisionName,
                         Players = new List<PlayerListItem>() //If players need to be fetched here just bolt on another join
                     },
                     registeredOn = r.RegisteredOn,
                     registrationStatus = r.Status,
                     statusDate = r.StatusDate
                 }
                ).ToListAsync();
        }
        /*
        public async Task<List<TeamListItem>> GetAllTeamsAsync()
            //New idea, *Registration-first*. TeamListItem is now specifically for registrations so I can make
            //  a separate viewmodel for teams specifically.
        {
            return await
                (from t in _db.Team
                 join d in _db.Division on t.TeamDivision equals d.DivisionId
                /*(from t in _db.Team
                 join d in _db.Division on t.TeamDivision equals d.DivisionId
                 join r in _db.Registration on t.TeamId equals r.TeamId into registrations
                 from r in registrations.DefaultIfEmpty()
                 select new TeamListItem
                 {
                     id = t.TeamId,
                     teamName = t.TeamName,
                     teamDivision = t.TeamDivision,
                     divisionName = d.DivisionName,
                     IsPaid = r.Status,
                     DatePaid = r.StatusDate
                 })
                .ToListAsync();
                
        }
        */

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
            var team = await
                (from t in _db.Team
                 join d in _db.Division on t.TeamDivision equals d.DivisionId
                 where t.TeamId == id
                 select new TeamListItem
                 {
                     id = t.TeamId,
                     teamName = t.TeamName,
                     teamDivision = t.TeamDivision,
                     divisionName = d.DivisionName,
                     isPaid = t.RegistrationPaid,
                     paymentDate = t.PaymentDate,
                 }
                ).FirstOrDefaultAsync();

            if (team != null)
            {
                team.Players = await GetTeamPlayersAsync(team);
            }
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
