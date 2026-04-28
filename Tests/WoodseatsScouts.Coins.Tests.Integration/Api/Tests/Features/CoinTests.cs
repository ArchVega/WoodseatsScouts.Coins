using Shouldly;
using WoodseatsScouts.Coins.Api.Models.Requests.Coins;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Tests.Features;

public class CoinTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task AddingCoins()
    {
        var coins = (await CoinsClient.GetCoins()).Data;
        
        coins.Count.ShouldBe(0);
        
        await CoinsClient.CreateCoins(1, 10, 13);

        coins = (await CoinsClient.GetCoins()).Data;
        
        coins.Count.ShouldBe(10);
    }
}