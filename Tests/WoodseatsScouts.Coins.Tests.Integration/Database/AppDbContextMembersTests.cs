using WoodseatsScouts.Coins.Api.Data;
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
    
    //// PREVIOUS UNIT TESTS FOR CONTROLLER, NOW FOR INTEGRATION TESTS
    //  [Fact]
    // public void AddPointsToMember_ValidCoinCodes_Success()
    // {
    //     var pointsForMemberViewModel = new PointsForMemberViewModel { CoinCodes = ["C0001001010", "C0001001020"] };
    //     var result = Should.NotThrow(() => appDbContext.AddPointsToMember(9, pointsForMemberViewModel));
    //     
    //     result.ShouldBeOfType<CreatedAtActionResult>();
    // }
    //
    // [Fact]
    // public void AddPointsToMember_AnIncorrectCoinCode_ThrowsException()
    // {
    //     var appDbContextMock = new Mock<IAppDbContext>();
    //     var imagePersisterMock = new Mock<IImagePersister>();
    //     var membersController = new MembersController(appDbContextMock.Object, imagePersisterMock.Object);
    //
    //     var scavengedCoins = new List<ScavengedCoin> { new() { PointValue = 13 }, new() { PointValue = 6 } };
    //     var scavengeResults = new List<ScavengeResult> { new() { ScavengedCoins = scavengedCoins } };
    //     SetupDbMock(appDbContextMock, x => x.ScavengedCoins!, scavengedCoins);
    //     SetupDbMock(appDbContextMock, x => x.ScavengeResults!, scavengeResults);
    //     SetupDbMock(appDbContextMock, x => x.Members!, [
    //         new Member { Id = 9 }
    //     ]);
    //     
    //     var pointsForMemberViewModel = new PointsForMemberViewModel { CoinCodes = ["C0001001010", "test-invalid-coin-code"] };
    //     var result = membersController.AddPointsToMember(9, pointsForMemberViewModel);
    //     
    //     result.ShouldBeOfType<BadRequestObjectResult>();
    //     ((BadRequestObjectResult)result).Value.ShouldBe("Could not translate Coin Code 'test-invalid-coin-code'");
    // }
}