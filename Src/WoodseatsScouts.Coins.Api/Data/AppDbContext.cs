using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;
using WoodseatsScouts.Coins.Api.Models.View.Members;

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

        public DbSet<ScoutGroup>? ScoutGroups { get; set; }

        public DbSet<Section>? Sections { get; set; }

        public DbSet<Coin>? Coins { get; set; }

        public DbSet<ActivityBase>? ActivityBases { get; set; }

        public DbSet<ScavengedCoin>? ScavengedCoins { get; set; }

        public DbSet<ScavengeResult>? ScavengeResults { get; set; }

        public DbSet<ErrorLog>? ErrorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            const string coinCodeFormat = "'C' + (FORMAT([ActivityBaseSequenceNumber], '0000'))  + (FORMAT([ActivityBaseId], '000')) + (FORMAT([Value], '000'))";
            const string memberCodeFormat = "'M' + (FORMAT(ScoutGroupId, '000'))  + [SectionId] + (FORMAT(Number, '000'))";

            /* As of the v2024, Coins data is generated externally and the Id value is predetermined and inserted. */
            modelBuilder.Entity<Coin>()
                .Property(x => x.Code).HasComputedColumnSql(coinCodeFormat);

            modelBuilder.Entity<Coin>().HasOne(x => x.ActivityBase).WithMany().HasForeignKey(x => x.ActivityBaseId);

            /* See property notes */
            modelBuilder.Entity<Member>()
                .Property(p => p.Code)
                .HasComputedColumnSql(memberCodeFormat);

            modelBuilder.Entity<Section>()
                .HasIndex(u => u.Code)
                .IsUnique();

            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 1, Name = "Archery" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 2, Name = "Abseiling" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 3, Name = "Aerial Trek" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 4, Name = "Aeroball" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 5, Name = "Bouldering" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 6, Name = "Bushcraft" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 7, Name = "Campfire" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 8, Name = "Canoeing" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 9, Name = "Caving" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 10, Name = "Fencing" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 11, Name = "Hike" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 12, Name = "Hillwalking" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 13, Name = "Kayaking" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 14, Name = "Orienteering" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 15, Name = "Pioneering" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 16, Name = "Powerboating" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 17, Name = "Raft Building" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 18, Name = "Sailing" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 19, Name = "Tomahawk throwing" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 20, Name = "Zip wire" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 99, Name = "Misc" });
        }

        public int GenerateNextMemberCode(int scoutGroupId, string section)
        {
            var matchingMembers = Members!.Where(x => x.ScoutGroupId == scoutGroupId && x.SectionId == section).ToList();

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

        public List<MemberPointsSummaryDto> GetLatestScans(int numberOfScans)
        {
            var last6MembersIdsWithCompletedAtTimes = ScavengeResults!
                .Include(x => x.Member)
                .OrderByDescending(x => x.CompletedAt)
                .Take(numberOfScans)
                .ToList();

            var f = last6MembersIdsWithCompletedAtTimes.Select(scavengeResult =>
            {
                var member = Members!
                    .Include(x => x.Section)
                    .Include(x => x.ScoutGroup)
                    .Include(x => x.ScavengeResults)
                    .ThenInclude(x => x.ScavengedCoins)
                    .ThenInclude(x => x.Coin)
                    .ThenInclude(x => x.ActivityBase)
                    .Single(x => x.Id == scavengeResult.MemberId);

                var viewModel = new MemberPointsSummaryDto(member)
                {
                    SelectedHaulResultId = scavengeResult.Id
                };

                return viewModel;
            }).ToList();

            return f;
        }

        public List<GroupPoints> GetTopThreeGroupsInLastHour()
        {
            var now = TimeProvider.GetLocalNow().DateTime;
            var startDateTime = now.AddHours(-1);
            return TopXScoutGroupsSinceY(3, startDateTime, now);
        }

        public List<GroupPoints> GetGroupsWithMostPoints()
        {
            return TopXScoutGroupsSinceY(10, leaderboardSettings.ScavengerHuntStartTime, leaderboardSettings.ScavengerHuntDeadline);
        }

        private List<GroupPoints> TopXScoutGroupsSinceY(int count, DateTime startDateTime, DateTime endDateTime)
        {
            var memberIds = ScavengeResults!
                .Include(x => x.Member)
                .Where(x => x.CompletedAt >= startDateTime)
                .Select(x => x.MemberId)
                .ToList();

            var memberGroupedByScoutGroup = Members!
                .Include(x => x.ScoutGroup)
                .Include(x => x.ScavengeResults)
                .ThenInclude(x => x.ScavengedCoins)
                .Where(x => memberIds.Contains(x.Id))
                .ToList()
                .GroupBy(x => x.ScoutGroupId)
                .ToList();

            var topXGroupsInLastYHours = (
                from grouping in memberGroupedByScoutGroup
                let sum = grouping.SelectMany(x => x.ScavengeResults).SelectMany(x => x.ScavengedCoins).Sum(x => x.Coin.Value) // changed
                select new GroupPoints
                {
                    Id = grouping.First().ScoutGroup.Id,
                    Name = grouping.First().ScoutGroup.Name,
                    TotalPoints = sum
                }).ToList();

            var allScoutGroupsWithMembers = ScoutGroups!.Include(x => x.Members).ToList();
            topXGroupsInLastYHours.ForEach(x =>
                x.MemberCount = allScoutGroupsWithMembers.Single(y => y.Id == x.Id).Members.Count);

            topXGroupsInLastYHours
                = topXGroupsInLastYHours.OrderByDescending(x => x.AveragePoints).Take(count).ToList();

            return topXGroupsInLastYHours;
        }

        public ScoutGroup CreateScoutGroup(int id, string name)
        {
            using var transaction = Database.BeginTransaction();
            Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScoutGroups ON");

            var scoutGroup = new ScoutGroup
            {
                Id = id,
                Name = name
            };
            ScoutGroups?.Add(scoutGroup);

            SaveChanges();

            Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScoutGroups OFF");
            transaction.Commit();

            return scoutGroup;
        }

        public ScavengeResult CreateScavengeResult(Member member)
        {
            var scavengeResult = new ScavengeResult
            {
                MemberId = member.Id,
                CompletedAt = DateTime.UtcNow
            };

            ScavengeResults!.Add(scavengeResult);

            SaveChanges();

            return scavengeResult;
        }

        public void CreateScavengedCoins(ScavengeResult scavengeResult, List<string> coinCodes)
        {
            // changed
            foreach (var coinCode in coinCodes)
            {
                // var result = CodeTranslator.TranslateCoinCode(coinCode);
                //
                // ScavengedCoins!.Add(new ScavengedCoin
                // {
                //     ScavengeResultId = scavengeResult.Id,
                //     BaseNumber = result.BaseNumber,
                //     PointValue = result.PointValue,
                //     Code = coinCode
                // });

                var coin = Coins!.Single(x => x.Code == coinCode);

                ScavengedCoins!.Add(new ScavengedCoin
                {
                    ScavengeResultId = scavengeResult.Id,
                    CoinId = coin.Id
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
                    coinToAdd.LockUntil = DateTime.UtcNow.AddMinutes(appSettingsOptions.Value.MinutesToLockScavengedCoins);
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

        public Member CreateMember(string firstName, string lastName, int scoutGroupId, string sectionId, bool isDayVisitor)
        {
            var member = new Member
            {
                FirstName = firstName,
                LastName = lastName,
                ScoutGroupId = scoutGroupId,
                SectionId = sectionId,
                IsDayVisitor = isDayVisitor,
                Number = GenerateNextMemberCode(scoutGroupId, sectionId)
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

        public List<Coin> CreateCoins(int baseId, int points, int count)
        {
            var maxBaseValueId = Coins!.Where(x => x.ActivityBaseId == baseId).Max(x => x.ActivityBaseSequenceNumber);
            var newCoins = new List<Coin>();
            for (var i = maxBaseValueId + 1; i < maxBaseValueId + 1 + count; i++)
            {
                newCoins.Add(new Coin
                {
                    ActivityBaseId = baseId,
                    ActivityBaseSequenceNumber = i,
                    Value = points
                });
            }

            Coins!.AddRange(newCoins);
            SaveChanges();

            return newCoins;
        }
    }
}