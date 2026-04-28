using Shouldly;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Tests.Features;

public class ScoutGroupTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateScoutGroups()
    {
        var scoutGroupDtos = (await ScoutGroupsClient.GetScoutGroups()).Data;
        
        scoutGroupDtos.Count.ShouldBe(0);

        await ScoutGroupsClient.PostScoutGroup("test-1");
        
        scoutGroupDtos = (await ScoutGroupsClient.GetScoutGroups()).Data;
        
        scoutGroupDtos.Count.ShouldBe(1);
    }
}