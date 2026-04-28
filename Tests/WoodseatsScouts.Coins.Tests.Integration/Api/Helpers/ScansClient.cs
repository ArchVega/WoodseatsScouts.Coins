using System.Net.Http.Json;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scans;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Helpers;

public class ScansClient(HttpClient client) 
{
    public async Task<ApiResult<List<ScannedCoinDto>>> GetScannedCoinDtos() 
    {
        var response = await client.GetAsync("/api/scans/coins");
     
        var data = (await response.Content.ReadFromJsonAsync<List<ScannedCoinDto>>())!;

        return new ApiResult<List<ScannedCoinDto>>(data, response);
    }
    
    public async Task<ApiResult<List<ScannedCoinDto>>> PutScannedCoin() 
    {
        throw new NotImplementedException();
        var response = await client.GetAsync("/api/scans/coins");
     
        var data = (await response.Content.ReadFromJsonAsync<List<ScannedCoinDto>>())!;

        return new ApiResult<List<ScannedCoinDto>>(data, response);
    }
}