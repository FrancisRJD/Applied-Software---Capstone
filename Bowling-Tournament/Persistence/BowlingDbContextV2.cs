using System.Numerics;
using bowling_tournament_MVCPRoject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Persistence
    //New and improved DbContext made for providing a handle into mucking with the new bowling-tournament-db-v2
    //  database.
{
    public class BowlingDbContextV2 : DbContext
    {
        public BowlingDbContextV2(DbContextOptions<BowlingDbContextV2> options)
           : base(options)
        {

        }

        public DbSet<Player> Player => Set<Player>();
        public DbSet<Registration> Registration => Set<Registration>();
        public DbSet<TeamV2> Team => Set<TeamV2>();
        public DbSet<Tournament> Tournament => Set<Tournament>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            //Overriding the default handling of this just to make absolutely sure that TeamV2 is properly
            //  mapping to the Team table
        {
            modelBuilder.Entity<Player>().ToTable("Player").HasKey(x=>x.PlayerId);
            modelBuilder.Entity<Registration>().ToTable("Registration").HasKey(x => x.RegistrationId);
            modelBuilder.Entity<TeamV2>().ToTable("Team").HasKey(x => x.TeamId);
            modelBuilder.Entity<Tournament>().ToTable("Tournament").HasKey(x => x.TournamentId);

            //Unique ID safety assurance since *all* of these tables can have updates, creates, and deletes
            modelBuilder.Entity<Player>()
                .HasIndex(x => x.PlayerId)
                .IsUnique();
            modelBuilder.Entity<Registration>()
                .HasIndex(x => x.TournamentId)
                .IsUnique();
            modelBuilder.Entity<TeamV2>()
                .HasIndex(x => x.TeamId)
                .IsUnique();
            modelBuilder.Entity<Tournament>()
                .HasIndex(x => x.TournamentId)
                .IsUnique();

            //Time conversion handling (Converts date to UtcDateTime)
            modelBuilder.Entity<Registration>()
                .Property(x => x.StatusDate)
                .HasConversion(
                    v => v.ToUniversalTime().ToString("yyyy-MM-dd"),
                    v => DateTimeOffset.Parse(v).UtcDateTime
                );
        }
    }
}
