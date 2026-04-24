using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

namespace WoodseatsScouts.Coins.Api.Services;

public class MemberService(IAppDbContext appDbContext) : IMemberService
{
    public int GetMemberId(int memberNumber, int scoutGroupNumber, string? sectionId)
    {
        return appDbContext.ScoutMembers!
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .Single(x => x.Number == memberNumber && x.ScoutGroupId == scoutGroupNumber && x.ScoutSectionId == sectionId)
            .Id;
    }

    public MemberDto GetMemberDto(int memberId)
    {
        var member = appDbContext.ScoutMembers!.Single(x => x.Id == memberId);

        return new MemberDto(member);
    }

    public MemberPointsSummaryDto MemberPointsSummaryDto(int memberId)
    {
        throw new NotImplementedException();
        // return new MemberPointsSummaryDto();
    }

    public List<MemberPointsSummaryDto> GetMemberWithPointsSummaryDtos()
    {
        return appDbContext.ScoutMembers!
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScanCoins)
            .ThenInclude(x => x.Coin)
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .ToList()
            .Select(x => new MemberPointsSummaryDto(x))
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToList();
    }

    public MemberCompleteSummaryDto MemberCompleteSummaryDto(int memberId)
    {
        return appDbContext.ScoutMembers!
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScanCoins)
            .ThenInclude(x => x.Coin)
            .ThenInclude(x => x.ActivityBase)
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .Where(x => x.Id == memberId)
            .ToList()
            .Select(x => new MemberCompleteSummaryDto(x))
            .First();
    }
}