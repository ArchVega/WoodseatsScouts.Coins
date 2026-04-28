using System.Net.Http.Json;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts;
using WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Groups;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Helpers;

public class ScoutGroupsClient(HttpClient client)
{
    public async Task<ApiResult<List<ScoutGroupDto>>> GetScoutGroups()
    {
        var response = await client.GetAsync("/api/scouts/groups");

        var data = (await response.Content.ReadFromJsonAsync<List<ScoutGroupDto>>())!;

        return new ApiResult<List<ScoutGroupDto>>(data, response);
    }

    public async Task<ScoutGroupDto> PostScoutGroup(string name)
    {
        var response = await client.PostAsJsonAsync(
            "/api/scouts/groups",
            new CreateScoutGroupRequest
            {
                Name = name
            });

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<ScoutGroupDto>())!;
    }
}