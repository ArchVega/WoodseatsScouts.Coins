using System.Net.Http.Json;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Helpers;

public class ActivityBasesClient(HttpClient client)
{
    public async Task<ApiResult<List<ActivityBasesDto>>> GetActivityBases()
    {
        var response = await client.GetAsync("/api/activities/bases");
     
        var data = (await response.Content.ReadFromJsonAsync<List<ActivityBasesDto>>())!;

        return new ApiResult<List<ActivityBasesDto>>(data, response);
    }
}