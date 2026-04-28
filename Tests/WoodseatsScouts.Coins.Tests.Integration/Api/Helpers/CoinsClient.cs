using System.Net.Http.Json;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Helpers;

public class CoinsClient(HttpClient client)
{
    public async Task<HttpResponseMessage> PostBooks(List<List<CoinDto>> coins)
    {
        return await client.PostAsJsonAsync("/api/coins", coins);
    }
}