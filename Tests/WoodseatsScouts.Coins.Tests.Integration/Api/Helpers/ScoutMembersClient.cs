using System.Net.Http.Json;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;
using WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Members;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Helpers;

public class ScoutMembersClient(HttpClient client)
{
    public async Task<ApiResult<List<ScoutMemberDto>>> GetScoutMembers()
    {
        var response = await client.GetAsync("/api/scouts/members");

        var data = (await response.Content.ReadFromJsonAsync<List<ScoutMemberDto>>())!;

        return new ApiResult<List<ScoutMemberDto>>(data, response);
    }

    public async Task<ScoutMemberDto> PostScoutMember(string firstname, string lastname, int scoutGroupId, string scoutSectionCode)
    {
        var response = await client.PostAsJsonAsync(
            "/api/scouts/members",
            new CreateMemberRequest
            {
                FirstName = firstname,
                LastName = lastname,
                ScoutGroupId = scoutGroupId,
                SectionCode = scoutSectionCode,
                IsDayVisitor = true
            });

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<ScoutMemberDto>())!;
    }

    public async Task<AddPointsToMemberDto> AssignCoinsToScoutMember(int id, List<string> coinCodes)
    {
        var response = await client.PutAsJsonAsync(
            $"/api/scouts/members/{id}/coins",
            new AssignCoinsToScoutMembersRequest
            {
                CoinCodes = coinCodes
            });

        response.EnsureSuccessStatusCode();
        
        return (await response.Content.ReadFromJsonAsync<AddPointsToMemberDto>())!;
    }

    public async Task<ApiResult<ScoutMemberCompleteSummaryDto>> GetScoutMemberComplete(int scoutMemberId)
    {
        var response = await client.GetAsync($"/api/scouts/members/{scoutMemberId}/complete");

        var data = (await response.Content.ReadFromJsonAsync<ScoutMemberCompleteSummaryDto>())!;

        return new ApiResult<ScoutMemberCompleteSummaryDto>(data, response);
    }
}