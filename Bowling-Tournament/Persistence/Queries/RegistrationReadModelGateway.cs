using bowling_tournament_MVCPRoject.Domain.Entities;
using bowling_tournament_MVCPRoject.UI.Queries;
using bowling_tournament_MVCPRoject.UI.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Persistence.Queries
{
    public class RegistrationReadModelGateway : IRegistrationReadModelGateway
    {
        private readonly BowlingDbContextV2 _db;
        public RegistrationReadModelGateway(BowlingDbContextV2 context)
        {
            _db = context;
        }

        public async Task<List<RegistrationListItem>> GetAllAsync()
        {
            return await
                (from r in _db.Registration
                 join te in _db.Team on r.TeamId equals te.TeamId
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
                         registrationOpen = to.RegistrationOpen
                     },
                     team = new TeamListItem
                     {
                         id = te.TeamId,
                         teamName = te.TeamName,
                         teamDivision = te.TeamDivision,
                         isPaid = te.RegistrationPaid,
                         paymentDate = te.PaymentDate ?? new DateTime(),
                     },
                     registeredOn = r.RegisteredOn,
                     registrationStatus = r.Status,
                     statusDate = r.StatusDate,
                 }
                ).ToListAsync();
        }

        public async Task<List<RegistrationListItem>> GetAllPaidAsync()
        {
            return await (
                from r in _db.Registration
                join te in _db.Team on r.TeamId equals te.TeamId
                join to in _db.Tournament on r.TournamentId equals to.TournamentId
                where te.RegistrationPaid == true
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
                        registrationOpen = to.RegistrationOpen
                    },
                    team = new TeamListItem
                    {
                        id = te.TeamId,
                        teamName = te.TeamName,
                        teamDivision = te.TeamDivision,
                        isPaid = te.RegistrationPaid,
                        paymentDate = te.PaymentDate ?? new DateTime(),
                    },
                    registeredOn = r.RegisteredOn,
                    registrationStatus = r.Status,
                    statusDate = r.StatusDate,
                }).ToListAsync();
        }

        public async Task<RegistrationListItem?> GetByIdAsync(int id)
        {
            return await (
                from r in _db.Registration
                join te in _db.Team on r.TeamId equals te.TeamId
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
                        registrationOpen = to.RegistrationOpen
                    },
                    team = new TeamListItem
                    {
                        id = te.TeamId,
                        teamName = te.TeamName,
                        teamDivision = te.TeamDivision,
                    },
                    registeredOn = r.RegisteredOn,
                    registrationStatus = r.Status,
                    statusDate = r.StatusDate,
                }).FirstOrDefaultAsync(r => r.id == id);
        }

        public async Task<List<RegistrationListItem>> GetWaitlistedByTournamentAsync(int tournamentId)
        {
            // First: Execute the query without the optional Division join
            var registrations = await (
                from r in _db.Registration
                join te in _db.Team on r.TeamId equals te.TeamId
                join to in _db.Tournament on r.TournamentId equals to.TournamentId
                where r.TournamentId == tournamentId && r.Status == RegistrationStatus.Waitlisted
                orderby r.RegisteredOn ascending
                select new
                {
                    registration = r,
                    team = te,
                    tournament = to
                }
            ).ToListAsync();

            // Second: Join with Division in-memory and map to RegistrationListItem
            var divisions = await _db.Division.ToListAsync();

            var result = registrations.Select(x => new RegistrationListItem
            {
                id = x.registration.RegistrationId,
                tournament = new TournamentListItem
                {
                    id = x.tournament.TournamentId,
                    tournamentName = x.tournament.TournamentName,
                    tournamentDate = x.tournament.TournamentDate,
                    location = x.tournament.Location,
                    teamCapacity = x.tournament.TeamCapacity,
                    watcherCapacity = x.tournament.WatcherCapacity,
                    registrationOpen = x.tournament.RegistrationOpen
                },
                team = new TeamListItem
                {
                    id = x.team.TeamId,
                    teamName = x.team.TeamName,
                    teamDivision = x.team.TeamDivision,
                    divisionName = divisions.FirstOrDefault(d => d.DivisionId == x.team.TeamDivision)?.DivisionName,
                    isPaid = x.team.RegistrationPaid,
                    paymentDate = x.team.PaymentDate ?? new DateTime(),
                },
                registeredOn = x.registration.RegisteredOn,
                registrationStatus = x.registration.Status,
                statusDate = x.registration.StatusDate,
            }).ToList();

            return result;
        }
    }
}
