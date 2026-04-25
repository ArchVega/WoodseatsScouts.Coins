using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IScoutMemberService
{
    int GetScoutMemberId(int scoutMemberNumber, int scoutGroupNumber, string? scoutSectionId);
    
    ScoutMemberDto GetMemberDto(int scoutMemberId);
    
    ScoutMemberCompleteSummaryDto MemberCompleteSummaryDto(int scoutMemberId);
    
    ScoutMemberPointsSummaryDto ScoutMemberPointsSummaryDto(int scoutMemberId);
    
    List<ScoutMemberPointsSummaryDto> ScoutGetMemberWithPointsSummaryDtos();
}