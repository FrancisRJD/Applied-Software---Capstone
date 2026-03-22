using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ReadModels;
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
    }
}
