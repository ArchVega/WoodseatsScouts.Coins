using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Data
{
    public class AppDbContext(
        DbContextOptions<AppDbContext> options,
        IOptions<AppSettings> appSettingsOptions)
        : DbContext(options), IAppDbContext
    {
        public TimeProvider TimeProvider { get; set; } = TimeProvider.System;

        public DbSet<ScoutMember> ScoutMembers { get; set; }

        public DbSet<ScoutGroup> ScoutGroups { get; set; }

        public DbSet<ScoutSection> ScoutSections { get; set; }

        public DbSet<Coin> Coins { get; set; }

        public DbSet<ActivityBase> ActivityBases { get; set; }

        public DbSet<ScanCoin> ScanCoins { get; set; }

        public DbSet<ScanSession> ScanSessions { get; set; }

        public DbSet<ErrorLog> ErrorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // const string coinCodeFormat = "'C' + (FORMAT([ActivityBaseSequenceNumber], '0000'))  + (FORMAT([ActivityBaseId], '000')) + (FORMAT([Value], '000'))";
            // const string memberCodeFormat = "'M' + (FORMAT(ScoutGroupId, '000'))  + [ScoutSectionCode] + (FORMAT(Number, '000'))";

            const string coinCodeFormat = """
                                              'C'
                                              + RIGHT('0000' + CAST([ActivityBaseSequenceNumber] AS VARCHAR(4)), 4)
                                              + RIGHT('000' + CAST([ActivityBaseId] AS VARCHAR(3)), 3)
                                              + RIGHT('000' + CAST([Value] AS VARCHAR(3)), 3)
                                          """;
            const string memberCodeFormat = """
                                                'M'
                                                + RIGHT('000' + CAST([ScoutGroupId] AS VARCHAR(3)), 3)
                                                + ScoutSectionCode
                                                + RIGHT('000' + CAST([Number] AS VARCHAR(3)), 3)
                                            """;

            /* As of the v2024, Coins data is generated externally and the Id value is predetermined and inserted. */
            modelBuilder.Entity<Coin>()
                .Property(x => x.Code)
                .HasComputedColumnSql(coinCodeFormat);

            modelBuilder.Entity<Coin>()
                .HasIndex(x => x.Code)
                .IsUnique();

            modelBuilder.Entity<Coin>().HasOne(x => x.ActivityBase).WithMany().HasForeignKey(x => x.ActivityBaseId);

            /* See property notes */
            modelBuilder.Entity<ScoutMember>()
                .Property(p => p.Code)
                .HasComputedColumnSql(memberCodeFormat);

            modelBuilder.Entity<ScoutMember>()
                .HasIndex(x => x.Code)
                .IsUnique();

            modelBuilder.Entity<ScoutSection>()
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
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 21, Name = "46th St Pauls" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 22, Name = "146th Old Norton" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 23, Name = "173rd Woodhouse" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 24, Name = "186th Manor" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 25, Name = "219th Stradbroke" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 26, Name = "229th Greenhill" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 27, Name = "246th Beauchief" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 28, Name = "270th Intake" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 29, Name = "273rd Handsworth" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 31, Name = "280th Norton" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 32, Name = "297th Bradway" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 33, Name = "49th Beighton" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 34, Name = "69th Mosborough" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 35, Name = "74th Oak Street" });
            modelBuilder.Entity<ActivityBase>().HasData(new ActivityBase { Id = 99, Name = "Misc" });
        }

        public int GenerateNextMemberCode(int scoutGroupId, string section)
        {
            var matchingMembers = ScoutMembers!.Where(x => x.ScoutGroupId == scoutGroupId && x.ScoutSectionCode == section).ToList();

            var nextMemberNumber = 1;
            if (matchingMembers.Count > 0)
            {
                nextMemberNumber = matchingMembers.Max(x => x.Number) + 1;
            }

            return nextMemberNumber;
        }

        public List<ScoutMember> GetLastThreeUsersToScanPoints()
        {
            return ScoutMembers!
                .Include(x => x.ScanSessions)
                .ThenInclude(x => x.ScanCoins)
                .Include(x => x.ScoutSection)
                .Where(x => x.ScanSessions.Count > 0)
                .Select(member => new
                {
                    Member = member,
                    LatestScavengeResult = ScanSessions!
                        .Where(y => y.ScoutMemberId == member.Id)
                        .OrderByDescending(x => x.CompletedAt)
                        .First()
                })
                .OrderByDescending(x => x.LatestScavengeResult.CompletedAt)
                .Take(3)
                .Select(x => x.Member)
                .ToList();
        }

        public List<ScoutMemberPointsSummaryDto> GetLatestScans(int numberOfScans)
        {
            var last6MembersIdsWithCompletedAtTimes = ScanSessions!
                .Include(x => x.ScoutMember)
                .OrderByDescending(x => x.CompletedAt)
                .Take(numberOfScans)
                .ToList();

            var memberPointsSummaryDtos = last6MembersIdsWithCompletedAtTimes.Select(scavengeResult =>
            {
                var member = ScoutMembers!
                    .Include(x => x.ScoutSection)
                    .Include(x => x.ScoutGroup)
                    .Include(x => x.ScanSessions)
                    .ThenInclude(x => x.ScanCoins)
                    .ThenInclude(x => x.Coin)
                    .ThenInclude(x => x!.ActivityBase)
                    .Single(x => x.Id == scavengeResult.ScoutMemberId);

                var memberPointsSummaryDto = new ScoutMemberPointsSummaryDto(member)
                {
                    SelectedHaulResultId = scavengeResult.Id
                };

                return memberPointsSummaryDto;
            }).ToList();

            return memberPointsSummaryDtos;
        }

        public ScoutGroup CreateScoutGroup(string name)
        {
            using var transaction = Database.BeginTransaction();

            var scoutGroup = new ScoutGroup
            {
                Name = name
            };
            ScoutGroups.Add(scoutGroup);

            SaveChanges();

            transaction.Commit();

            return scoutGroup;
        }

        public ScanSession CreateScavengeResult(ScoutMember scoutMember)
        {
            var scavengeResult = new ScanSession
            {
                ScoutMemberId = scoutMember.Id,
                CompletedAt = DateTime.UtcNow
            };

            ScanSessions!.Add(scavengeResult);

            SaveChanges();

            return scavengeResult;
        }

        public void CreateScavengedCoins(ScanSession scanSession, List<string> coinCodes)
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

                ScanCoins!.Add(new ScanCoin
                {
                    ScanSessionId = scanSession.Id,
                    CoinId = coin.Id
                });
            }
        }

        public List<Coin> RecordMemberAgainstUnscavengedCoins(ScoutMember scoutMember, List<string> coinCodes)
        {
            var alreadyScavengedCoins = new List<Coin>();

            foreach (var coinCode in coinCodes)
            {
                var coinToAdd = Coins!.Single(x => x.Code == coinCode);
                if (coinToAdd.MemberId == null)
                {
                    coinToAdd.MemberId = scoutMember.Id;
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
                alreadyScavengedCoins.ForEach(coin => coin.Member = ScoutMembers!.Single(x => x.Id == coin.MemberId));
            }

            return alreadyScavengedCoins;
        }

        public ScoutMember CreateMember(string firstName, string lastName, int scoutGroupId, string scoutSectionId, bool isDayVisitor)
        {
            var member = new ScoutMember
            {
                FirstName = firstName,
                LastName = lastName,
                ScoutGroupId = scoutGroupId,
                ScoutSectionCode = scoutSectionId,
                IsDayVisitor = isDayVisitor,
                Number = GenerateNextMemberCode(scoutGroupId, scoutSectionId)
            };

            ScoutMembers?.Add(member);

            SaveChanges();

            return member;
        }

        public ScoutMember UpdateMemberName(int memberId, string firstName, string lastName)
        {
            var member = ScoutMembers!.Single(x => x.Id == memberId);

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
                    Value = points,
                    ActivityBase = null
                });
            }

            Coins!.AddRange(newCoins);
            SaveChanges();

            return newCoins;
        }
    }
}