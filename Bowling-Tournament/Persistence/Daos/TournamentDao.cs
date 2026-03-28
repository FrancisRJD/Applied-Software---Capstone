using bowling_tournament_MVCPRoject.Domain.Daos;
using bowling_tournament_MVCPRoject.Domain.Entities;

namespace bowling_tournament_MVCPRoject.Persistence.Daos
{
    public class TournamentDao : ITournamentDao
    {
        private readonly BowlingDbContextV2 _db;

        public TournamentDao(BowlingDbContextV2 db)
        {
            _db = db;
        }

        public void addTournament(Tournament tournament)
        {
            _db.Tournament.Add(tournament);
            _db.SaveChanges();
        }

        public Tournament findTournament(Tournament tournament)
        {
            return _db.Tournament.Find(tournament.TournamentId) ?? new Tournament { TournamentId = -1 };
        }

        public void removeTournament(Tournament tournament)
        {
            _db.Tournament.Remove(tournament);
            _db.SaveChanges();
        }

        public void saveChanges()
        {
            _db.SaveChanges();
        }
    }
}
