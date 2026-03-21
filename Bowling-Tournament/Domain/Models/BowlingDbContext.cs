using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace bowling_tournament_MVCPRoject.Domain.Models
{
    public class BowlingDbContext : DbContext
    {
        public BowlingDbContext(DbContextOptions<BowlingDbContext> options)
           : base(options)
        {
        }

        public DbSet<BowlingUser> BowlingUser => Set<BowlingUser>();

        public DbSet<Division> Division => Set<Division>();

        public DbSet<Team> Team => Set<Team>();

        public DbSet<Player> Player => Set<Player>();

    }
}
