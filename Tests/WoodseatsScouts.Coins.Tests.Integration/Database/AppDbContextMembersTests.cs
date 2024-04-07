using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Tests.Integration.Helpers;
using Xunit.Abstractions;

namespace WoodseatsScouts.Coins.Tests.Integration.Database;

[Collection("Database collection")]
public class AppDbContextMembersTests
{
    private readonly AppDbContext appDbContext;
    private readonly TestDataFactory testDataFactory;
    private readonly DatabaseFixture databaseFixture;
    private readonly ITestOutputHelper testOutputHelper;
    
    public AppDbContextMembersTests(DatabaseFixture databaseFixture, ITestOutputHelper testOutputHelper)
    {
        this.databaseFixture = databaseFixture;
        this.testOutputHelper = testOutputHelper;
        appDbContext = databaseFixture!.AppDbContext;
        testDataFactory = new TestDataFactory(appDbContext);
        
        /* Setting arbitrary start and end dates*/
        databaseFixture.LeaderboardSettings.ScavengerHuntStartTime = DateTime.Now.AddDays(-1);
        databaseFixture.LeaderboardSettings.ScavengerHuntDeadline = DateTime.Now.AddDays(1);
        databaseFixture.AppSettings.MinutesToLockScavengedCoins = 10;
    }
    
    [Fact]
    public void Test1()
    {
        databaseFixture.RestoreBaseTestData();
        
        var member = appDbContext.Members.First();
        var tallyHistoryItem = appDbContext.CreateScavengeResult(member);
    }
    
    [Fact]
    public void Test2()
    {
        databaseFixture.RestoreBaseTestData();
        
        var member = appDbContext.Members.First();
        var coins = appDbContext.Coins.Take(3);
        var tallyHistoryItem = appDbContext.CreateScavengeResult(member);
        appDbContext.CreateScavengedCoins(tallyHistoryItem, coins.Select(x => x.Code).ToList());
    }
    
    [Fact]
    public void Test3()
    {
        databaseFixture.RestoreBaseTestData();
        
        var member = appDbContext.Members.First();
        var coins = appDbContext.Coins.Take(3);
        var tallyHistoryItem = appDbContext.CreateScavengeResult(member);
        appDbContext.CreateScavengedCoins(tallyHistoryItem, coins.Select(x => x.Code).ToList());
        var alreadyScavengedCoins = appDbContext.RecordMemberAgainstUnscavengedCoins(member, coins.Select(x => x.Code).ToList());
    }

    [Fact]
    public void Test4()
    {
        databaseFixture.RestoreBaseTestData();
        
        var member = appDbContext!.Members!.First();
        
        var coins = appDbContext.Coins!.Take(3);
        var tallyHistoryItem = appDbContext.CreateScavengeResult(member);
        appDbContext.CreateScavengedCoins(tallyHistoryItem, coins.Select(x => x.Code).ToList());
        var alreadyScavengedCoins = appDbContext.RecordMemberAgainstUnscavengedCoins(member, coins.Select(x => x.Code).ToList());
    }
}