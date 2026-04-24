using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IMemberService
{
    bool HasMemberImage(int memberId);
    
    int GetMemberId(int memberNumber, int scoutGroupNumber, string? sectionId);
    
    MemberDto GetMemberDto(int member);
    
    MemberCompleteSummaryDto MemberCompleteSummaryDto(int memberId);
    
    MemberPointsSummaryDto MemberPointsSummaryDto(int memberId);
    
    List<MemberPointsSummaryDto> GetMemberWithPointsSummaryDtos();
}