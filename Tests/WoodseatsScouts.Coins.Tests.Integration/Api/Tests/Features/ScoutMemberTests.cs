using Shouldly;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Tests.Features;

public class ScoutMemberTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateScoutMembers()
    {
        var scoutMemberDtos = (await ScoutMembersClient.GetScoutMembers()).Data;
        
        scoutMemberDtos.Count.ShouldBe(0);
        
        
        
        scoutMemberDtos = (await ScoutMembersClient.GetScoutMembers()).Data;
        
        scoutMemberDtos.Count.ShouldBe(0);
    }
}