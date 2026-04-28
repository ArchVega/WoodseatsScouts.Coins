using Shouldly;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Tests.Features;

public class ScoutSectionTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateScoutSections()
    {
        var scoutSectionDtos = (await ScoutSectionsClient.GetScoutSections()).Data;
        
        scoutSectionDtos.Count.ShouldBe(0);

        await ScoutSectionsClient.PostScoutSection("t", "test-1");
        await ScoutSectionsClient.PostScoutSection("r", "test-2");
        
        scoutSectionDtos = (await ScoutSectionsClient.GetScoutSections()).Data;
        
        scoutSectionDtos.Count.ShouldBe(2);
    }
}