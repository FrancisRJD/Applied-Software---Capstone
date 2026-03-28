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

        public Registration? findRegistrationbyTeamAndTournament(int teamId, int tournamentId)
        {
            return _db.Registration
                .Where(te => te.TeamId == teamId)
                .Where(to => to.TournamentId == tournamentId)
                .FirstOrDefault();
        }

        public List<Registration> getRegistrationsByTournament(int tournamentId)
        {
            return _db.Registration
                .Where(r => r.TournamentId == tournamentId)
                .Select(tr => new Registration
                {
                    RegistrationId = tr.RegistrationId,
                    TournamentId = tr.TournamentId,
                    TeamId = tr.TeamId,
                    RegisteredOn = tr.RegisteredOn,
                    Status = tr.Status,
                    StatusDate = tr.StatusDate
                })
                .ToList();
        }

        public List<Registration> getRegistrationsByStatus(int status)
        {

            return _db.Registration
                .Where(r => r.Status == (RegistrationStatus) status)
                .Select(tr => new Registration
                {
                    RegistrationId = tr.RegistrationId,
                    TournamentId = tr.TournamentId,
                    TeamId= tr.TeamId,
                    RegisteredOn = tr.RegisteredOn,
                    Status = tr.Status,
                    StatusDate = tr.StatusDate

                })
                .ToList();
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
            var found = _db.Registration.FirstOrDefault(r => r.RegistrationId == registration.RegistrationId);

            if (found == null)
                return;

            found.TournamentId = registration.TournamentId;
            found.TeamId = registration.TeamId;
            found.RegisteredOn = registration.RegisteredOn;
            found.Status = registration.Status;
            found.StatusDate = registration.StatusDate;

            _db.SaveChanges();
        }

        public Registration? findById(int id)
        {
            return _db.Registration.FirstOrDefault(r => r.RegistrationId == id);
        }
    }
}
