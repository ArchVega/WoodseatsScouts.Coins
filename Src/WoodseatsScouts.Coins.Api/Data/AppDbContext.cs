using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Data
{
    public class AppDbContext(
        DbContextOptions<AppDbContext> options,
        IOptions<AppSettings> appSettingsOptions,
        SystemDateTimeProvider systemDateTimeProvider,
        IOptions<LeaderboardSettings> leaderboardSettings)
        : DbContext(options), IAppDbContext
    {
        private readonly LeaderboardSettings leaderboardSettings = leaderboardSettings.Value;
        public TimeProvider TimeProvider { get; set; } = TimeProvider.System;

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
                .Property(et => et.Id)
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
            var now = TimeProvider.GetLocalNow().DateTime;
            var startDateTime = now.AddHours(-1);
            return TopXTroopsSinceY(3, startDateTime, now);
        }

        public List<GroupPoints> GetGroupsWithMostPoints()
        {
            return TopXTroopsSinceY(10, leaderboardSettings.ScavengerHuntStartTime, leaderboardSettings.ScavengerHuntDeadline);
        }

        private List<GroupPoints> TopXTroopsSinceY(int count, DateTime startDateTime, DateTime endDateTime)
        {
            var memberIds = ScavengeResults!
                .Include(x => x.Member)
                .Where(x => x.CompletedAt >= startDateTime)
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

        public Troop CreateTroop(int id, string name)
        {
            using var transaction = Database.BeginTransaction();
            Database.ExecuteSqlRaw("SET IDENTITY_INSERT Troops ON");

            var troop = new Troop
            {
                Id = id,
                Name = name
            };
            Troops?.Add(troop);

            SaveChanges();

            Database.ExecuteSqlRaw("SET IDENTITY_INSERT Troops OFF");
            transaction.Commit();

            return troop;
        }

        public ScavengeResult CreateScavengeResult(Member member)
        {
            var scavengeResult = new ScavengeResult
            {
                MemberId = member.Id,
                CompletedAt = DateTime.Now
            };

            ScavengeResults!.Add(scavengeResult);

            SaveChanges();

            return scavengeResult;
        }

        public void CreateScavengedCoins(ScavengeResult scavengeResult, List<string> coinCodes)
        {
            foreach (var coinCode in coinCodes)
            {
                var result = CodeTranslator.TranslateCoinCode(coinCode);

                ScavengedCoins!.Add(new ScavengedCoin
                {
                    ScavengeResultId = scavengeResult.Id,
                    BaseNumber = result.BaseNumber,
                    PointValue = result.PointValue,
                    Code = coinCode
                });
            }
        }

        public List<Coin> RecordMemberAgainstUnscavengedCoins(Member member, List<string> coinCodes)
        {
            var alreadyScavengedCoins = new List<Coin>();
            
            foreach (var coinCode in coinCodes)
            {
                var coinToAdd = Coins!.Single(x => x.Code == coinCode);
                if (coinToAdd.MemberId == null)
                {
                    coinToAdd.MemberId = member.Id;
                    coinToAdd.LockUntil = DateTime.Now.AddMinutes(appSettingsOptions.Value.MinutesToLockScavengedCoins);
                }
                else
                {
                    alreadyScavengedCoins.Add(coinToAdd);
                }
            }

            SaveChanges();

            if (alreadyScavengedCoins.Count > 0)
            {
                alreadyScavengedCoins.ForEach(coin => coin.Member = Members!.Single(x => x.Id == coin.MemberId));
            }
            
            return alreadyScavengedCoins;
        }

        public Member CreateMember(string firstName, string lastName, int troopId, string sectionId, bool isDayVisitor)
        {
            var member = new Member
            {
                FirstName = firstName,
                LastName = lastName,
                TroopId = troopId,
                SectionId = sectionId,
                IsDayVisitor = isDayVisitor,
                Number = GenerateNextMemberCode(troopId, sectionId)
            };

            Members?.Add(member);

            SaveChanges();

            return member;
        }

        public Member UpdateMemberName(int memberId, string firstName, string lastName)
        {
            var member = Members!.Single(x => x.Id == memberId);

            member.FirstName = firstName;
            member.LastName = lastName;
            SaveChanges();

            return member;
        }
    }
}