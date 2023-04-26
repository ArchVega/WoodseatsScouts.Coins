using Shouldly;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.Tests.Integration;

[Collection("Database collection")]
public class AppDbContextTestsReports
{
    private readonly DatabaseFixture fixture;

    public AppDbContextTestsReports(DatabaseFixture fixture)
    {
        this.fixture = fixture;
        this.fixture.ResetCoins();
    }
    
    #region GetTopThreeGroupsInLastHour
    
    [Fact]
    public void GetTopThreeGroupsInLastHour_NoScavengedResults_ResultsShouldBeEmpty()
    {
        var topThreeGroupsInLastHour = fixture.AppDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(0);
    }

    [Fact]
    public void GetTopThreeGroupsInLastHour_OneScavengeResults_ResultsShouldBe1()
    {
        var now = DateTime.Now;
        var points = new List<int> { 20 };
        var expectedSum = points.Sum();
        CreateScavengedResult(TestDataFactory.Troop1Member1, now, points);

        var topThreeGroupsInLastHour = fixture.AppDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(1);
        topThreeGroupsInLastHour.First().TotalPoints.ShouldBe(expectedSum);
    }

    [Fact]
    public void GetTopThreeGroupsInLastHour_FourScavengeResults_ResultsShouldBe3()
    {
        var differentGroupMembers = new List<Member>()
        {
            TestDataFactory.Troop1Member1,
            TestDataFactory.Troop2Member1,
            TestDataFactory.Troop3Member1,
            TestDataFactory.Troop4Member1,
        };
        
        var expectedSums = new Dictionary<int, int>();
        
        for (int i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = DateTime.Now;
            var points = new List<int> { 20, 10 * i };
            expectedSums.Add(i, points.Sum());
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topThreeGroupsInLastHour = fixture.AppDbContext.GetTopThreeGroupsInLastHour();
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
            TestDataFactory.Troop1Member1,
            TestDataFactory.Troop2Member1,
            TestDataFactory.Troop3Member1,
            TestDataFactory.Troop4Member1,
        };
        
        for (int i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = i % 2 == 0 ? DateTime.Now : DateTime.Now.AddMinutes(-61);
            
            var points = new List<int> { 10 };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topThreeGroupsInLastHour = fixture.AppDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(2);
    }
    
    [Fact]
    public void GetTopThreeGroupsInLastHour_MultipleUsersInSameGroup_AveragePointsIsCorrectlyCalculated()
    {
        var differentGroupMembers = new List<Member>()
        {
            TestDataFactory.Troop1Member1,
            TestDataFactory.Troop1Member2,
            TestDataFactory.Troop1Member3
        };

        var eachMemberPoint = 20;
        
        for (int i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = DateTime.Now;
            var points = new List<int> { eachMemberPoint };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topThreeGroupsInLastHour = fixture.AppDbContext.GetTopThreeGroupsInLastHour();
        topThreeGroupsInLastHour.Count.ShouldBe(1);
        topThreeGroupsInLastHour.First().TotalPoints.ShouldBe(60);
        topThreeGroupsInLastHour.First().AveragePoints.ShouldBe(20);
    }
    
    #endregion

    #region GetGroupsWithMostPointsThisWeekend

    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_NoScavengedResults_ResultsShouldBeEmpty()
    {
        var topGroupsThisWeekend = fixture.AppDbContext.GetGroupsWithMostPointsThisWeekend();
        topGroupsThisWeekend.Count.ShouldBe(0);
    }

    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_FourScavengeResultsButOnly2OnTheWeekend_ResultsShouldBe2()
    {
        var differentGroupMembers = new List<Member>()
        {
            TestDataFactory.Troop1Member1,
            TestDataFactory.Troop2Member1,
            TestDataFactory.Troop3Member1,
            TestDataFactory.Troop4Member1,
        };

        var saturday = DateTime.Parse("22/04/2023");
        var thursday = DateTime.Parse("20/04/2023 23:59:59");
        for (int i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = i % 2 == 0 ? saturday : thursday; // 
            
            var points = new List<int> { 10 };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topGroupsThisWeekend = fixture.AppDbContext.GetGroupsWithMostPointsThisWeekend();
        topGroupsThisWeekend.Count.ShouldBe(2);
    }
    
    [Fact]
    public void GetGroupsWithMostPointsThisWeekend_FourScavengeResultsThatStartsAt1AMFriday_ResultsShouldBe2()
    {
        var differentGroupMembers = new List<Member>()
        {
            TestDataFactory.Troop1Member1,
            TestDataFactory.Troop2Member1,
            TestDataFactory.Troop3Member1,
            TestDataFactory.Troop4Member1,
        };

        var saturday = DateTime.Parse("22/04/2023");
        var friday = DateTime.Parse("21/04/2023 01:00:00");
        for (int i = 0; i < differentGroupMembers.Count; i++)
        {
            var now = i % 2 == 0 ? saturday : friday; // 
            
            var points = new List<int> { 10 };
            CreateScavengedResult(differentGroupMembers[i], now, points);
        }
        
        var topGroupsThisWeekend = fixture.AppDbContext.GetGroupsWithMostPointsThisWeekend();
        topGroupsThisWeekend.Count.ShouldBe(4);
    }
    
    #endregion
    
    #region GetLastThreeUsersToScanPoints
    
    [Fact]
    public void GetLastThreeUsersToScanPoints_NoScavengedResults_ResultsShouldBeEmpty()
    {
        var lastThreeUsersToScanPoints = fixture.AppDbContext.GetLastThreeUsersToScanPoints();
        lastThreeUsersToScanPoints.Count.ShouldBe(0);
    }
    
    [Fact]
    public void GetLastThreeUsersToScanPoints_4MembersScan_OnlyLatestThreeShown()
    {
        var members = new List<Member>
        {
            TestDataFactory.Troop1Member1,
            TestDataFactory.Troop1Member2,
            TestDataFactory.Troop1Member3,
            TestDataFactory.Troop1Member4,
        };
        
        var dateTime = DateTime.Now;
        for (int i = 0; i < members.Count; i++)
        {
            var now = dateTime.AddMinutes(i);
            var points = new List<int> { 20, 10 * i };
            CreateScavengedResult(members[i], now, points);
        }
        
        var lastThreeUsersToScanPoints = fixture.AppDbContext.GetLastThreeUsersToScanPoints();
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

        fixture.AppDbContext.SaveChanges();

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

            fixture.AppDbContext.ScavengedCoins!.Add(scavengedCoin);
        }

        fixture.AppDbContext.SaveChanges();
    }
}