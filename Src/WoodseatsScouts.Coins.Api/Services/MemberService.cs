using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

namespace WoodseatsScouts.Coins.Api.Services;

public class MemberService(IAppDbContext appDbContext) : IMemberService
{
    public Member GetMember(int memberNumber, int scoutGroupNumber, string? sectionId)
    {
        return appDbContext.Members!
            .Include(x => x.ScoutGroup)
            .Include(x => x.Section)
            .Single(x => x.Number == memberNumber
                         && x.ScoutGroupId == scoutGroupNumber
                         && x.SectionId == sectionId);
    }

    public MemberCompleteSummaryDto MemberCompleteSummaryDto(Member member)
    {
        return new MemberCompleteSummaryDto();
    }
    
    public MemberPointsSummaryDto MemberPointsSummaryDto(Member member)
    {
        return new MemberPointsSummaryDto();
    }
}