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
            return await (
                from r in _db.Registration
                join te in _db.Team on r.TeamId equals te.TeamId
                join to in _db.Tournament on r.TournamentId equals to.TournamentId
                join d in _db.Division on te.TeamDivision equals d.DivisionId into divisionGroup
                from d in divisionGroup.DefaultIfEmpty()
                where r.TournamentId == tournamentId && r.Status == RegistrationStatus.Waitlisted
                orderby r.RegisteredOn ascending
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
                        divisionName = d.DivisionName,
                        isPaid = te.RegistrationPaid,
                        paymentDate = te.PaymentDate ?? new DateTime(),
                    },
                    registeredOn = r.RegisteredOn,
                    registrationStatus = r.Status,
                    statusDate = r.StatusDate,
                }).ToListAsync();
        }
    }
}
