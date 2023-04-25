﻿using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.App.Models;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Member>? Members { get; set; }

        public DbSet<Troop>? Troops { get; set; }

        public DbSet<ScavengedCoin>? ScavengedCoins { get; set; }

        public DbSet<ScavengeResult>? ScavengeResults { get; set; }
        
        public DbSet<ErrorLog>? ErrorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .Property(p => p.Code)
                .HasComputedColumnSql("'M' + (FORMAT(TroopId, '000'))  + [Section] + (FORMAT(Number, '000'))");

            DbContextSeedFactory.CreateUsers(modelBuilder);
        }

        public int GenerateNextMemberCode(int troopId, string section)
        {
            var matchingMembers = Members!.Where(x => x.TroopId == troopId && x.Section == section).ToList();

            var nextMemberNumber = 1;
            if (matchingMembers.Count > 0)
            {
                nextMemberNumber = matchingMembers.Max(x => x.Number) + 1;
            }

            return nextMemberNumber;
        }

        public List<Member> GetLastThreeUsersToScanPoints()
        {
            var f = Members!
                .Include(x => x.ScavengeResults)
                .ThenInclude(x => x.ScavengedCoins)
                .Where(x => x.ScavengeResults.Count > 0)
                .Select(member => new
                {
                    Member = member,
                    LatestScavengeResult = ScavengeResults!.Where(y => y.MemberId == member.Id).Max(y => y.CompletedAt)
                }).ToList();

            return f.OrderByDescending(x => x.LatestScavengeResult)
                .Take(3)
                .Select(x => x.Member)
                .ToList();
        }

        public List<GroupTotalPoints> GetTopThreeGroupsInLastHour()
        {
            var startDateTime = DateTime.Now.AddHours(-1);
            return TopXTroopsSinceY(3, startDateTime);
        }

        public List<GroupTotalPoints> GetGroupsWithMostPointsThisWeekend()
        {
            var startDateTime = GetPreviousFriday();
            return TopXTroopsSinceY(10, startDateTime);
        }

        private List<GroupTotalPoints> TopXTroopsSinceY(int count, DateTime startDateTime)
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

            var topThreeGroupsInLastHour = (
                from grouping in memberGroupedByTroop
                let sum =
                    grouping.SelectMany(x => x.ScavengeResults).SelectMany(x => x.ScavengedCoins).Sum(x => x.PointValue)
                select new GroupTotalPoints { Name = grouping.First().Troop.Name, TotalPoints = sum }).ToList();

            topThreeGroupsInLastHour =
                topThreeGroupsInLastHour.OrderByDescending(x => x.TotalPoints).Take(count).ToList();

            return topThreeGroupsInLastHour;
        }

        private DateTime GetPreviousFriday()
        {
            for (int i = 0; i < 7; i++)
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