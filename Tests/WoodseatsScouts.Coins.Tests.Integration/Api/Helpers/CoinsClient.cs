using System.Net.Http.Json;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Api.Models.Requests.Coins;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Helpers;

public class CoinsClient(HttpClient client)
{
    public async Task<ApiResult<List<CoinDto>>> GetCoins()
    {
        var response = await client.GetAsync("/api/coins");

        var data = (await response.Content.ReadFromJsonAsync<List<CoinDto>>())!;

        return new ApiResult<List<CoinDto>>(data, response);
    }

    public async Task<List<CoinDto>> CreateCoins(int activityBaseId, int pointsPerCoin, int numberToCreate)
    {
        var response = await client.PostAsJsonAsync(
            "/api/coins",
            new CreateCoinsRequest
            {
                ActivityBaseId = activityBaseId,
                PointsPerCoin = pointsPerCoin,
                NumberToCreate = numberToCreate
            });
        
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<List<CoinDto>>())!;
    }

    // public async Task AssignCoinToMember(string coinCode, string scoutMemberCode)
    // {
    //     var response = await client.PutAsJsonAsync($"/api/coins/{coinCode}/assign/{scoutMemberCode}", new StringContent(string.Empty));
    //     
    //     response.EnsureSuccessStatusCode();
    // }
}