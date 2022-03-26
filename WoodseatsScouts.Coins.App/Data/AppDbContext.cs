using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ScoutPoint>? ScoutPoints { get; set; }

        public DbSet<Scout>? Scouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DbContextSeedFactory.CreateUsers(modelBuilder);
        }
    }
}
