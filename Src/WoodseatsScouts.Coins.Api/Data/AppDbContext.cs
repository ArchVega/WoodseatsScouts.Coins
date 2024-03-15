using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Models;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Data
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Member>? Members { get; set; }

        public DbSet<Troop>? Troops { get; set; }
        
        public DbSet<Section>? Sections { get; set; }
        
        public DbSet<Coin>? Coins { get; set; }

        public DbSet<ScavengedCoin>? ScavengedCoins { get; set; }

        public DbSet<ScavengeResult>? ScavengeResults { get; set; }
        

        public DbSet<ErrorLog>? ErrorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* As of the v2024, Coins data is generated externally and the Id value is predetermined and inserted. */
            modelBuilder.Entity<Coin>()
                .Property( et => et.Id )
                .ValueGeneratedNever();
            
            /* See property notes */
            modelBuilder.Entity<Member>()
                .Property(p => p.Code)
                .HasComputedColumnSql("'M' + (FORMAT(TroopId, '000'))  + [SectionId] + (FORMAT(Number, '000'))");

            modelBuilder.Entity<Section>()
                .HasIndex(u => u.Code)
                .IsUnique();
        }

        public int GenerateNextMemberCode(int troopId, string section)
        {
            var matchingMembers = Members!.Where(x => x.TroopId == troopId && x.SectionId == section).ToList();

            var nextMemberNumber = 1;
            if (matchingMembers.Count > 0)
            {
                nextMemberNumber = matchingMembers.Max(x => x.Number) + 1;
            }

            return nextMemberNumber;
        }

        public List<Member> GetLastThreeUsersToScanPoints()
        {
            return Members!
                .Include(x => x.ScavengeResults)
                .ThenInclude(x => x.ScavengedCoins)
                .Include(x => x.Section)
                .Where(x => x.ScavengeResults.Count > 0)
                .Select(member => new
                {
                    Member = member,
                    LatestScavengeResult = ScavengeResults!
                        .Where(y => y.MemberId == member.Id)
                        .OrderByDescending(x => x.CompletedAt)
                        .First()
                })
                .OrderByDescending(x => x.LatestScavengeResult.CompletedAt)
                .Take(3)
                .Select(x => x.Member)
                .ToList();
        }

        public List<GroupPoints> GetTopThreeGroupsInLastHour()
        {
            var startDateTime = DateTime.Now.AddHours(-1);
            return TopXTroopsSinceY(3, startDateTime);
        }

        public List<GroupPoints> GetGroupsWithMostPointsThisWeekend()
        {
            var startDateTime = GetPreviousFriday();
            return TopXTroopsSinceY(10, startDateTime);
        }

        private List<GroupPoints> TopXTroopsSinceY(int count, DateTime startDateTime)
        {
            var memberIds = ScavengeResults!
                .Include(x => x.Member)
                .Where(x => x.CompletedAt > startDateTime)
                .Select(x => x.MemberId)
                .ToList();

            var memberGroupedByTroop = Members!
                .Include(x => x.Troop)
                .Include(x => x.ScavengeResults)
                .ThenInclude(x => x.ScavengedCoins)
                .Where(x => memberIds.Contains(x.Id))
                .ToList()
                .GroupBy(x => x.TroopId)
                .ToList();

            var topXGroupsInLastYHours = (
                from grouping in memberGroupedByTroop
                let sum =
                    grouping.SelectMany(x => x.ScavengeResults).SelectMany(x => x.ScavengedCoins).Sum(x => x.PointValue)
                select new GroupPoints
                {
                    Id = grouping.First().Troop.Id,
                    Name = grouping.First().Troop.Name,
                    TotalPoints = sum
                }).ToList();

            var allTroopsWithMembers = Troops!.Include(x => x.Members).ToList();
            topXGroupsInLastYHours.ForEach(x =>
                x.MemberCount = allTroopsWithMembers.Single(y => y.Id == x.Id).Members.Count);

            topXGroupsInLastYHours
                = topXGroupsInLastYHours.OrderByDescending(x => x.AveragePoints).Take(count).ToList();

            return topXGroupsInLastYHours;
        }

        private static DateTime GetPreviousFriday()
        {
            for (var i = 0; i < 7; i++)
            {
                var testDate = DateTime.Today.AddDays(-1 * i);
                if (testDate.DayOfWeek == DayOfWeek.Friday)
                {
                    return testDate;
                }
            }

            throw new InvalidOperationException("Could not find the last Friday of the current week");
        }
    }
}