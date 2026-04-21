using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

namespace WoodseatsScouts.Coins.Api.Services;

public class MemberService(IAppDbContext appDbContext) : IMemberService
{
    public bool HasMemberImage(int memberId)
    {
        return appDbContext.Members!.Single(x => x.Id == memberId).HasImage;
    }

    public int GetMemberId(int memberNumber, int scoutGroupNumber, string? sectionId)
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

        return new MemberDto(member);
    }

    public MemberPointsSummaryDto MemberPointsSummaryDto(int memberId)
    {
        throw new NotImplementedException();
        // return new MemberPointsSummaryDto();
    }

    public List<MemberPointsSummaryDto> GetMemberWithPointsSummaryDtos()
    {
        return appDbContext.Members!
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScavengedCoins)
            .ThenInclude(x => x.Coin)
            .Include(x => x.ScoutGroup)
            .Include(x => x.Section)
            .ToList()
            .Select(x => new MemberPointsSummaryDto(x))
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToList();
    }

    public MemberCompleteSummaryDto MemberCompleteSummaryDto(int memberId)
    {
        return appDbContext.Members!
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScavengedCoins)
            .ThenInclude(x => x.Coin)
            .ThenInclude(x => x.ActivityBase)
            .Include(x => x.ScoutGroup)
            .Include(x => x.Section)
            .Where(x => x.Id == memberId)
            .ToList()
            .Select(x => new MemberCompleteSummaryDto(x))
            .First();
    }
}