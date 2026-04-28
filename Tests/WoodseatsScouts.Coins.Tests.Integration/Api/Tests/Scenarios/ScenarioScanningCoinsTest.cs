using Shouldly;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Tests.Scenarios;

public class ScenarioScanningCoinsTest(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task MembersScanningCoins()
    {
        var scoutSectionA = await ScoutSectionsClient.PostScoutSection("a", "SectionA");
        var scoutGroupAlpha = await ScoutGroupsClient.PostScoutGroup("GroupAlpha");
        var scoutMemberAlpha = await ScoutMembersClient.PostScoutMember("Alpha", "Alpha", scoutGroupAlpha.Id, scoutSectionA.Code);
        var activityBase = await ActivityBasesClient.PostActivityBases("BaseAlpha");
        var coinDtos = await CoinsClient.CreateCoins(activityBase.Id, 10, 10);
        var scanSession = await ScoutMembersClient.AssignCoinsToScoutMember(scoutMemberAlpha.Id, coinDtos.Select(x => x.Code).ToList());
        
        scanSession.ScanSessionId.ShouldBe(1);
    }
}