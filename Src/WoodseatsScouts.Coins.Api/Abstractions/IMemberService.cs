using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IMemberService
{
    int GetScoutMemberId(int scoutMemberNumber, int scoutGroupNumber, string? scoutSectionId);
    
    ScoutScoutMemberDto GetMemberDto(int scoutMemberId);
    
    ScoutMemberCompleteSummaryDto MemberCompleteSummaryDto(int scoutMemberId);
    
    ScoutMemberPointsSummaryDto ScoutMemberPointsSummaryDto(int scoutMemberId);
    
    List<ScoutMemberPointsSummaryDto> ScoutGetMemberWithPointsSummaryDtos();
}