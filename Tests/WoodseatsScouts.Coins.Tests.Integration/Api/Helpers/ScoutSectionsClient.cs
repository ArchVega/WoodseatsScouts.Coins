using System.Net.Http.Json;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts;
using WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Groups;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Helpers;

public class ScoutSectionsClient(HttpClient client) 
{
    public async Task<ApiResult<List<ScoutSectionDto>>> GetScoutSections() 
    {
        var response = await client.GetAsync("/api/scouts/sections");
     
        var data = (await response.Content.ReadFromJsonAsync<List<ScoutSectionDto>>())!;

        return new ApiResult<List<ScoutSectionDto>>(data, response);
    }
    
    public async Task<ScoutSectionDto> PostScoutSection(string code, string name)
    {
        await client.PostAsJsonAsync("/api/scouts/sections", new CreateScoutSectionRequest
        {
            Code = code,
            Name = name
        });

        return (await GetScoutSections()).Data.Single(x => x.Code == code); // not really necessary, just following the convention.
    }
}