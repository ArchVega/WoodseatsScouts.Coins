using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IMemberService
{
    Member GetMember(int memberNumber, int scoutGroupNumber, string? sectionId);
    
    MemberCompleteSummaryDto MemberCompleteSummaryDto(Member member);
    
    MemberPointsSummaryDto MemberPointsSummaryDto(Member member);
}