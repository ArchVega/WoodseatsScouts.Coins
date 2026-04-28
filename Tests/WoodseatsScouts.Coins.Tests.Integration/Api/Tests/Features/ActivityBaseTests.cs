using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Tests.Features;

public class ActivityBaseTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task ActivityBases_Get_ReturnsList()
    {
        var apiResult = await ActivityBasesClient.GetActivityBases();

        Assert.Equal("Archery", apiResult.Data.First().Name);
    }
}