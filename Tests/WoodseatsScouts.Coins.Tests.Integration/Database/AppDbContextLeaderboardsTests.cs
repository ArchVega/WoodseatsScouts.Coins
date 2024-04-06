using Microsoft.Extensions.Time.Testing;
using Shouldly;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Tests.Integration.Helpers;
using Xunit.Abstractions;

namespace WoodseatsScouts.Coins.Tests.Integration.Database;

[Collection("Database collection")]
public class AppDbContextLeaderboardsTests
{
    private readonly AppDbContext appDbContext;
    private readonly TestDataFactory testDataFactory;
    private readonly DatabaseFixture databaseFixture;
    private readonly ITestOutputHelper testOutputHelper;

    public AppDbContextLeaderboardsTests(DatabaseFixture databaseFixture, ITestOutputHelper testOutputHelper)
    {
        this.databaseFixture = databaseFixture;
        this.testOutputHelper = testOutputHelper;
        appDbContext = databaseFixture!.AppDbContext;
        testDataFactory = new TestDataFactory(appDbContext);
        
        /* Setting arbitrary start and end dates*/
        databaseFixture.LeaderboardSettings.ScavengerHuntStartTime = DateTime.Now.AddDays(-1);
        databaseFixture.LeaderboardSettings.ScavengerHuntDeadline = DateTime.Now.AddDays(1);
    }

    #region GetTopThreeGroupsInLastHour
    
    [Fact]
    public void GetTopThreeGroupsInLastHour_NoScavengedResults_ResultsShouldBeEmpty()
    {
        databaseFixture.RestoreBaseTestData();

        var topThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(0);
    }

    [Fact]
    public void GetTopThreeGroupsInLastHour_OneScavengeResults_ResultsShouldBe1()
    {
        databaseFixture.RestoreBaseTestData();
        
        var now = DateTime.Now;
        var points = new List<int> { 20 };
        var expectedSum = points.Sum();
        CreateScavengedResult(testDataFactory.Members.AsparagusRoyal, now, points);

        var topThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(1);
        topThreeGroupsInLastHour.First().TotalPoints.ShouldBe(expectedSum);
    }

    [Fact]
    public void GetTopThreeGroupsInLastHour_FourScavengeResults_ResultsShouldBe3()
    {
        databaseFixture.RestoreBaseTestData();

        var differentGroupMembers = new List<Member>()
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.GlaucousJet,
            testDataFactory.Members.CharcoalCrimson,
            testDataFactory.Members.HunterSaffron,
        };
        
        var expectedSums = new Dictionary<Member, int>();
        
        for (var i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = DateTime.Now;
            var points = new List<int> { 20, 10 * i };
            expectedSums.Add(differentGroupMembers[i], points.Sum());
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(3);

        topThreeGroupsInLastHour.Select(x => x.Name).ShouldContain("Jet");
        topThreeGroupsInLastHour.Select(x => x.Name).ShouldContain("Crimson");
        topThreeGroupsInLastHour.Select(x => x.Name).ShouldContain("Saffron");
        
        foreach (var groupPoint in topThreeGroupsInLastHour)
        {
            var groupName = groupPoint.Name;
        }
        
        // var expectedResultOrder = expectedSums.OrderByDescending(x => x.Value).ToList();
        //
        // for (int i = 0; i < differentGroupMembers.Take(3).Count(); i++)
        // {
        //     var expectedSum = expectedResultOrder[i].Value;
        //     topThreeGroupsInLastHour.ElementAt(i).TotalPoints.ShouldBe(expectedSum);
        // }
    }
    
    [Fact]
    public void GetTopThreeGroupsInLastHour_FourScavengeResultsButOnly2InTheLastHour_ResultsShouldBe2()
    {
        var differentGroupMembers = new List<Member>()
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.GlaucousJet,
            testDataFactory.Members.CharcoalCrimson,
            testDataFactory.Members.HunterSaffron,
        };
        
        for (int i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = i % 2 == 0 ? DateTime.Now : DateTime.Now.AddMinutes(-61);
            
            var points = new List<int> { 10 };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(2);
    }
    
    [Fact]
    public void GetTopThreeGroupsInLastHour_MultipleUsersInSameGroup_AveragePointsIsCorrectlyCalculated()
    {
        databaseFixture.RestoreBaseTestData();

        var differentGroupMembers = new List<Member>()
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.CeriseRoyal,
            testDataFactory.Members.GhostRoyal
        };

        const int eachMemberPoint = 20;
        
        for (var i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = DateTime.Now;
            var points = new List<int> { eachMemberPoint };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(1);
        topThreeGroupsInLastHour.First().TotalPoints.ShouldBe(60);
        topThreeGroupsInLastHour.First().AveragePoints.ShouldBe(15); // There are 4 members in the group.
    }
    
    #endregion

    #region GetGroupsWithMostPointsThisWeekend

    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_NoScavengedResults_ResultsShouldBeEmpty()
    {
        databaseFixture.RestoreBaseTestData();

        var topGroupsThisWeekend = appDbContext.GetGroupsWithMostPoints();
        topGroupsThisWeekend.Count.ShouldBe(0);
    }
    
    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_FourScavengeResultsButTwoOccuredBeforeStartTime_ResultsShouldBe2()
    {
        databaseFixture.RestoreBaseTestData();
      
        var differentGroupMembers = new List<Member>()
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.GlaucousJet,
            testDataFactory.Members.CharcoalCrimson,
            testDataFactory.Members.HunterSaffron,
        };

        var onTime = databaseFixture.LeaderboardSettings.ScavengerHuntDeadline.AddHours(-1);
        var tooEarly = databaseFixture.LeaderboardSettings.ScavengerHuntStartTime.AddHours(-1);
        
        for (int i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = i % 2 == 0 ? tooEarly : onTime; 
            
            var points = new List<int> { 10 };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topGroupsThisWeekend = appDbContext.GetGroupsWithMostPoints();
        topGroupsThisWeekend.Count.ShouldBe(2);
    }
    
    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_FourScavengeResultsAllWithinStartAndEndTimes_ResultsShouldBe4()
    {
        databaseFixture.RestoreBaseTestData();
      
        var differentGroupMembers = new List<Member>()
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.GlaucousJet,
            testDataFactory.Members.CharcoalCrimson,
            testDataFactory.Members.HunterSaffron,
        };

        var early = databaseFixture.LeaderboardSettings.ScavengerHuntStartTime.AddHours(1);
        var late = databaseFixture.LeaderboardSettings.ScavengerHuntDeadline.AddHours(-1);
        
        for (var i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = i % 2 == 0 ? late : early; 
            
            var points = new List<int> { 10 };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topGroupsThisWeekend = appDbContext.GetGroupsWithMostPoints();
        topGroupsThisWeekend.Count.ShouldBe(4);
    }
    
    #endregion
    
    #region GetLastThreeUsersToScanPoints
    
    [Fact]
    public void GetLastThreeUsersToScanPoints_NoScavengedResults_ResultsShouldBeEmpty()
    {
        databaseFixture.RestoreBaseTestData();

        var lastThreeUsersToScanPoints = appDbContext.GetLastThreeUsersToScanPoints();
        lastThreeUsersToScanPoints.Count.ShouldBe(0);
    }
    
    [Fact]
    public void GetLastThreeUsersToScanPoints_4MembersScan_OnlyLatestThreeShown()
    {
        databaseFixture.RestoreBaseTestData();

        var members = new List<Member>
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.CeriseRoyal,
            testDataFactory.Members.GhostRoyal,
            testDataFactory.Members.JasperRoyal,
        };
        
        var dateTime = DateTime.Now;
        for (int i = 0; i < members.Count; i++)
        {
            var now = dateTime.AddMinutes(i);
            var points = new List<int> { 20, 10 * i };
            CreateScavengedResult(members[i], now, points);
        }
        
        var lastThreeUsersToScanPoints = appDbContext.GetLastThreeUsersToScanPoints();
        lastThreeUsersToScanPoints.Count.ShouldBe(3);
        lastThreeUsersToScanPoints.ElementAt(0).FirstName.ShouldBe(testDataFactory.Members.JasperRoyal.FirstName);
        lastThreeUsersToScanPoints.ElementAt(1).FirstName.ShouldBe(testDataFactory.Members.GhostRoyal.FirstName);
        lastThreeUsersToScanPoints.ElementAt(2).FirstName.ShouldBe(testDataFactory.Members.CeriseRoyal.FirstName);
    }
    
    #endregion
    
    private void CreateScavengedResult(Member member, DateTime completedAt, List<int> points)
    {
        var scavengeResults = new ScavengeResult
        {
            Member = member,
            CompletedAt = completedAt
        };

        appDbContext.SaveChanges();

        foreach (var point in points)
        {
            var p = point.ToString();
            if (point == 5)
            {
                p = "05";
            }

            var scavengedCoin = new ScavengedCoin
            {
                Code = "C0010" + p,
                BaseNumber = 1,
                PointValue = point,
                ScavengeResult = scavengeResults
            };

            appDbContext.ScavengedCoins!.Add(scavengedCoin);
        }

        appDbContext.SaveChanges();
    }
}