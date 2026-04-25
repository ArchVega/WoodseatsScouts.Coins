using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Services;

public class ScoutMemberService(IAppDbContext appDbContext) : IScoutMemberService
{
    public int GetScoutMemberId(int scoutMemberNumber, int scoutGroupNumber, string? scoutSectionId)
    {
        return appDbContext.ScoutMembers!
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .Single(x => x.Number == scoutMemberNumber && x.ScoutGroupId == scoutGroupNumber && x.ScoutSectionCode == scoutSectionId)
            .Id;
    }

    public ScoutMemberDto GetMemberDto(int scoutMemberId)
    {
        var member = appDbContext.ScoutMembers!.Single(x => x.Id == scoutMemberId);

        return new ScoutMemberDto(member);
    }

    public ScoutMemberPointsSummaryDto ScoutMemberPointsSummaryDto(int scoutMemberId)
    {
        throw new NotImplementedException();
        // return new MemberPointsSummaryDto();
    }

    public List<ScoutMemberPointsSummaryDto> ScoutGetMemberWithPointsSummaryDtos()
    {
        return appDbContext.ScoutMembers!
            .Include(x => x.ScanSessions)
            .ThenInclude(x => x.ScanCoins)
            .ThenInclude(x => x.Coin)
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .ToList()
            .Select(x => new ScoutMemberPointsSummaryDto(x))
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToList();
    }

    public ScoutMemberCompleteSummaryDto MemberCompleteSummaryDto(int scoutMemberId)
    {
        return appDbContext.ScoutMembers!
            .Include(x => x.ScanSessions)
            .ThenInclude(x => x.ScanCoins)
            .ThenInclude(x => x.Coin)
            .ThenInclude(x => x!.ActivityBase)
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .Where(x => x.Id == scoutMemberId)
            .ToList()
            .Select(x => new ScoutMemberCompleteSummaryDto(x))
            .First();
    }
}