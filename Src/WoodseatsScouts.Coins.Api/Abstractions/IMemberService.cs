using WoodseatsScouts.Coins.Api.Models.Dtos.Members;

namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IMemberService
{
    int GetMemberId(int memberNumber, int scoutGroupNumber, string? sectionId);
    
    ScoutMemberDto GetMemberDto(int member);
    
    MemberCompleteSummaryDto MemberCompleteSummaryDto(int memberId);
    
    MemberPointsSummaryDto MemberPointsSummaryDto(int memberId);
    
    List<MemberPointsSummaryDto> GetMemberWithPointsSummaryDtos();
}