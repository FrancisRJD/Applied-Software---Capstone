using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ReadModels;
using bowling_tournament_MVCPRoject.UI.ViewModels;
using bowling_tournament_MVCPRoject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Persistence.Queries
{
    public class TournamentReadModelGateway : ITournamentReadModelGateway
    {
        private readonly BowlingDbContextV2 _db;
        public TournamentReadModelGateway (BowlingDbContextV2 context)
        {
            _db = context;
        }

        public async Task<List<TournamentListItem>> GetAllAsync()
        {
            return await
                (from t in _db.Tournament
                 select new TournamentListItem
                 {
                     id = t.TournamentId,
                     tournamentName = t.TournamentName,
                     tournamentDate = t.TournamentDate,
                     location = t.Location,
                     teamCapacity = t.TeamCapacity,
                     watcherCapacity = t.WatcherCapacity,
                     registrationOpen = t.RegistrationOpen,
                     menCapacity = t.MensCapacity,
                     womenCapacity = t.WomensCapacity,
                     mixedCapacity = t.MixedCapacity,
                     youthCapacity = t.YouthCapacity,
                     seniorCapacity = t.SeniorCapacity
                 }
                ).ToListAsync();
        }

        public async Task<List<TournamentOptions>> GetTournamentOptionsAsync()
        {
            return await
                (from t in _db.Tournament
                 select new TournamentOptions
                 {
                     Id = t.TournamentId,
                     TournamentName = t.TournamentName
                 }
                ).ToListAsync();
        }

        public async Task<TournamentListItem?> GetByIdAsync(int id)
        {
            return await _db.Tournament
                .Where(t => t.TournamentId == id)
                .Select(t => new TournamentListItem
                {
                    id = t.TournamentId,
                    tournamentName = t.TournamentName,
                    tournamentDate = t.TournamentDate,
                    location = t.Location,
                    teamCapacity = t.TeamCapacity,
                    watcherCapacity = t.WatcherCapacity,
                    registrationOpen = t.RegistrationOpen,
                    menCapacity = t.MensCapacity,
                    womenCapacity = t.WomensCapacity,
                    mixedCapacity = t.MixedCapacity,
                    youthCapacity = t.YouthCapacity,
                    seniorCapacity = t.SeniorCapacity
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<TournamentWithCapacityVm>> GetAllWithCapacityAsync()
        {
            var tournaments = await GetAllAsync();
            var result = new List<TournamentWithCapacityVm>();

            foreach (var tournament in tournaments)
            {
                // Get registered (not waitlisted) teams for this tournament
                var registeredTeams = await (from r in _db.Registration
                                             join t in _db.Team on r.TeamId equals t.TeamId
                                             where r.TournamentId == tournament.id && r.Status == RegistrationStatus.Registered
                                             select t)
                                            .ToListAsync();

                // Count total registered teams
                int totalRegisteredCount = registeredTeams.Count;

                // Count teams by division
                int menCount = registeredTeams.Count(t => t.TeamDivision == 1);
                int womenCount = registeredTeams.Count(t => t.TeamDivision == 2);
                int mixedCount = registeredTeams.Count(t => t.TeamDivision == 3);
                int youthCount = registeredTeams.Count(t => t.TeamDivision == 4);
                int seniorCount = registeredTeams.Count(t => t.TeamDivision == 5);

                result.Add(new TournamentWithCapacityVm
                {
                    Id = tournament.id,
                    TournamentName = tournament.tournamentName,
                    TournamentDate = tournament.tournamentDate,
                    Location = tournament.location,
                    TeamCapacity = tournament.teamCapacity,
                    WatcherCapacity = tournament.watcherCapacity,
                    RegistrationOpen = tournament.registrationOpen,
                    MenCapacity = tournament.menCapacity,
                    WomenCapacity = tournament.womenCapacity,
                    MixedCapacity = tournament.mixedCapacity,
                    YouthCapacity = tournament.youthCapacity,
                    SeniorCapacity = tournament.seniorCapacity,
                    MenRemaining = tournament.menCapacity == -1 ? -1 : Math.Max(0, tournament.menCapacity - menCount),
                    WomenRemaining = tournament.womenCapacity == -1 ? -1 : Math.Max(0, tournament.womenCapacity - womenCount),
                    MixedRemaining = tournament.mixedCapacity == -1 ? -1 : Math.Max(0, tournament.mixedCapacity - mixedCount),
                    YouthRemaining = tournament.youthCapacity == -1 ? -1 : Math.Max(0, tournament.youthCapacity - youthCount),
                    SeniorRemaining = tournament.seniorCapacity == -1 ? -1 : Math.Max(0, tournament.seniorCapacity - seniorCount),
                    TeamRemaining = Math.Max(0, tournament.teamCapacity - totalRegisteredCount)
                });
            }

            return result;
        }
    }
}
