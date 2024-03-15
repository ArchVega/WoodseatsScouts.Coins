using Microsoft.Extensions.Time.Testing;
using Shouldly;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Tests.Integration.Helpers;

namespace WoodseatsScouts.Coins.Tests.Integration.Database;

[Collection("Database collection")]
public class AppDbContextLeaderboardsTests
{
    private readonly AppDbContext appDbContext;
    private readonly TestDataFactory testDataFactory;
    private readonly DatabaseFixture databaseFixture;

    public AppDbContextLeaderboardsTests(DatabaseFixture databaseFixture)
    {
        this.databaseFixture = databaseFixture;
        appDbContext = databaseFixture!.AppDbContext;
        testDataFactory = new TestDataFactory(appDbContext);
    }

    #region GetTopThreeGroupsInLastHour
    
    [Fact]
    public void GetTopThreeGroupsInLastHour_NoScavengedResults_ResultsShouldBeEmpty()
    {
        var topThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(0);
    }

    [Fact]
    public void GetTopThreeGroupsInLastHour_OneScavengeResults_ResultsShouldBe1()
    {
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
        var differentGroupMembers = new List<Member>()
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.GlaucousJet,
            testDataFactory.Members.CharcoalCrimson,
            testDataFactory.Members.HunterSaffron,
        };
        
        var expectedSums = new Dictionary<int, int>();
        
        for (var i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = DateTime.Now;
            var points = new List<int> { 20, 10 * i };
            expectedSums.Add(i, points.Sum());
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(3);

        var expectedResultOrder = expectedSums.OrderByDescending(x => x.Value).ToList();
        
        for (int i = 0; i < differentGroupMembers.Take(3).Count(); i++)
        {
            var expectedSum = expectedResultOrder[i].Value;
            topThreeGroupsInLastHour.ElementAt(i).TotalPoints.ShouldBe(expectedSum);
        }
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
        topThreeGroupsInLastHour.First().AveragePoints.ShouldBe(20);
    }
    
    #endregion

    #region GetGroupsWithMostPointsThisWeekend

    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_NoScavengedResults_ResultsShouldBeEmpty()
    {
        var topGroupsThisWeekend = appDbContext.GetGroupsWithMostPointsThisWeekend();
        topGroupsThisWeekend.Count.ShouldBe(0);
    }

    /// <summary>
    /// To prevent coins from being scanned after the deadline
    /// </summary>
    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_FourScavengeResultsButOnlyTwoOnTheWeekend_ResultsShouldBe2()
    {
        databaseFixture.RestoreBaseTestData();
        
        var fakeTimeProvider = new FakeTimeProvider();
        appDbContext.TimeProvider = fakeTimeProvider;
        
        var differentGroupMembers = new List<Member>()
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.GlaucousJet,
            testDataFactory.Members.CharcoalCrimson,
            testDataFactory.Members.HunterSaffron,
        };

        
        var tomorrow = fakeTimeProvider.GetLocalNow().AddDays(1).DateTime;
        var yesterday = fakeTimeProvider.GetLocalNow().AddDays(-1).DateTime;
        
        for (var i = 0; i < differentGroupMembers.Count; i++)
        {
            var scavengedAt = i % 2 == 0 ? tomorrow : yesterday;
            
            var points = new List<int> { 10 };
            CreateScavengedResult(differentGroupMembers[i], scavengedAt, points);
        }
        
        var topGroupsThisWeekend = appDbContext.GetGroupsWithMostPointsThisWeekend();
        topGroupsThisWeekend.Count.ShouldBe(2);
    }
    
    // todo test nameis wrong
    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_FourScavengeResultsThatStartsAt1AMFriday_ResultsShouldBe2()
    {
        databaseFixture.RestoreBaseTestData();
        
        var differentGroupMembers = new List<Member>()
        {
            testDataFactory.Members.AsparagusRoyal,
            testDataFactory.Members.GlaucousJet,
            testDataFactory.Members.CharcoalCrimson,
            testDataFactory.Members.HunterSaffron,
        };

        var saturday = DateTime.Parse("22/04/2023");
        var friday = DateTime.Parse("21/04/2023 01:00:00");
        for (int i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = i % 2 == 0 ? saturday : friday; // 
            
            var points = new List<int> { 10 };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topGroupsThisWeekend = appDbContext.GetGroupsWithMostPointsThisWeekend();
        topGroupsThisWeekend.Count.ShouldBe(4);
    }
    
    #endregion
    
    #region GetLastThreeUsersToScanPoints
    
    [Fact]
    public void GetLastThreeUsersToScanPoints_NoScavengedResults_ResultsShouldBeEmpty()
    {
        var lastThreeUsersToScanPoints = appDbContext.GetLastThreeUsersToScanPoints();
        lastThreeUsersToScanPoints.Count.ShouldBe(0);
    }
    
    [Fact]
    public void GetLastThreeUsersToScanPoints_4MembersScan_OnlyLatestThreeShown()
    {
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
        lastThreeUsersToScanPoints.ElementAt(0).FirstName.ShouldBe("Member4");
        lastThreeUsersToScanPoints.ElementAt(1).FirstName.ShouldBe("Member3");
        lastThreeUsersToScanPoints.ElementAt(2).FirstName.ShouldBe("Member2");
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
                Code = "B0010" + p,
                BaseNumber = 1,
                PointValue = point,
                ScavengeResult = scavengeResults
            };

            appDbContext.ScavengedCoins!.Add(scavengedCoin);
        }

        appDbContext.SaveChanges();
    }
}