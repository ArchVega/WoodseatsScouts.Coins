using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

namespace WoodseatsScouts.Coins.Api.Services;

public class MemberService(IAppDbContext appDbContext) : IMemberService
{
    public int GetMemberIdFromFragments(int memberNumber, int scoutGroupNumber, string? sectionId)
    {
        return appDbContext.Members!
            .Include(x => x.ScoutGroup)
            .Include(x => x.Section)
            .Single(x => x.Number == memberNumber && x.ScoutGroupId == scoutGroupNumber && x.SectionId == sectionId)
            .Id;
    }

    public MemberDto GetMemberDto(int memberId)
    {
        var member = appDbContext.Members!.Single(x => x.Id == memberId);

        return new MemberDto
        {
            Id = member.Id,
            Code = member.Code,
            Number = member.Number,
            FirstName = member.FirstName,
            LastName = member.LastName,
            FullName = member.FullName,
            ScoutGroupId = member.ScoutGroupId,
            ScoutGroupName = member.ScoutGroup.Name,
            SectionId = member.SectionId,
            SectionName = member.Section.Name,
            Clue1State = member.Clue1State,
            Clue2State = member.Clue2State,
            Clue3State = member.Clue3State,
            IsDayVisitor = member.IsDayVisitor,
            HasImage = member.HasImage,
        };
    }

    public MemberCompleteSummaryDto MemberCompleteSummaryDto(int memberId)
    {
        return new MemberCompleteSummaryDto();
    }

    public MemberPointsSummaryDto MemberPointsSummaryDto(int memberId)
    {
        return new MemberPointsSummaryDto();
    }
}