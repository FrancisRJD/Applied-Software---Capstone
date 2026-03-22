using bowling_tournament_MVCPRoject.Domain.Daos;
using bowling_tournament_MVCPRoject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace bowling_tournament_MVCPRoject.Persistence.Daos
{
    public class TournamentRegistrationDao : ITournamentRegistrationDao
    {
        private readonly BowlingDbContextV2 _db;

        public TournamentRegistrationDao(BowlingDbContextV2 db)
        {
            _db = db;
        }

        public void addRegistration(Registration registration)
        {
            _db.Registration.Add(registration);
            _db.SaveChanges();
        }

        public Registration findRegistrationbyTeamAndTournament(int teamId, int tournamentId)
        {
            return _db.Registration
                .Where(te => te.TeamId == teamId)
                .Where(to => to.TournamentId == tournamentId)
                .FirstOrDefault() ?? new Registration();
        }

        public async Task<List<Registration>> getRegistrationsByTournament(int tournamentId)
            //I realize more than likely unnecessary since this should be a readModel but bolting it here
        {
            return await _db.Registration
                .Where(r => r.TournamentId == tournamentId)
                .Select(tr => new Registration { 
                    RegistrationId = tr.RegistrationId,
                    TournamentId = tr.TournamentId,
                    TeamId = tr.TeamId,
                    RegisteredOn = tr.RegisteredOn,
                    Status = tr.Status,
                    StatusDate = tr.StatusDate
                }).ToListAsync();
        }

        public void removeRegistration(Registration registration)
        {
            _db.Registration.Remove(registration);
            _db.SaveChanges();
        }

        public void saveChanges()
        {
            _db.SaveChanges();
        }

        public void updateRegistration(Registration registration)
        {
            throw new NotImplementedException();
        }
    }
}
