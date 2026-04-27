using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Services;

public class ScoutMemberService(IAppDbContext appDbContext) : IScoutMemberService
{
    public int GetScoutMemberId(int scoutMemberNumber, int scoutGroupId, string? scoutSectionId)
    {
        var count = appDbContext.ScoutMembers!
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .Count(x => x.Number == scoutMemberNumber && x.ScoutGroupId == scoutGroupId && x.ScoutSectionCode == scoutSectionId);
        if (count > 1)
        {
            throw new Exception("Error");
        }

        return appDbContext.ScoutMembers!
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .Single(x => x.Number == scoutMemberNumber && x.ScoutGroupId == scoutGroupId && x.ScoutSectionCode == scoutSectionId)
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
        var scoutMemberCompleteSummaryDto = appDbContext.ScoutMembers!
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

        scoutMemberCompleteSummaryDto.ScoutMemberCompleteSummaryStatsDto.MostVisitedActivityBases = CalculateMostVisitedActivityBasesCount(scoutMemberId);
        scoutMemberCompleteSummaryDto.ScoutMemberCompleteSummaryStatsDto.LeastVisitedActivityBases = CalculateLeastVisitedActivityBasesCount(scoutMemberId);

        return scoutMemberCompleteSummaryDto;
    }

    private List<ScoutMemberCompleteSummaryStatsActivityBaseInfoDto> CalculateMostVisitedActivityBasesCount(int scoutMemberId)
    {
        var mostVisited = appDbContext
            .ScoutMembers
            .Where(x => x.Id == scoutMemberId)
            .SelectMany(x => x.ScanSessions)
            .SelectMany(hr => hr.ScanCoins)
            .Include(x => x.Coin)
            .Select(sc => sc.Coin!.ActivityBase)
            .Where(y => y != null)
            .GroupBy(activityBase => activityBase!.Name)
            .Select(g => new
            {
                ActivityBaseName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(3)
            .ToList();

        return mostVisited.Select(x => new ScoutMemberCompleteSummaryStatsActivityBaseInfoDto
        {
            Name = x.ActivityBaseName,
            TimesVisited = x.Count,
        }).ToList();
    }

    private List<ScoutMemberCompleteSummaryStatsActivityBaseInfoDto> CalculateLeastVisitedActivityBasesCount(int scoutMemberId)
    {
        /*  2026-04-27. Last minute feature to show least visited bases overall (which excludes the current participant's visits).
            Least bases is a special case. We want to get both the participant's least visited bases, as well the event's current least visited bases. Set by the caller.*/
        var leastVisited = appDbContext
            .ScoutMembers
            .Where(sm => sm.Id != scoutMemberId)
            .SelectMany(sm => sm.ScanSessions)
            .SelectMany(hr => hr.ScanCoins)
            .Include(x => x.Coin)
            .Select(sc => sc.Coin!.ActivityBase)
            .Where(y => y != null)
            .GroupBy(activityBase => activityBase!.Name)
            .Select(g => new
            {
                ActivityBaseName = g.Key,
                Count = g.Count()
            })
            .OrderBy(x => x.Count)
            .Take(3)
            .ToList();

        return leastVisited.Select(x => new ScoutMemberCompleteSummaryStatsActivityBaseInfoDto
        {
            Name = x.ActivityBaseName,
            TimesVisited = x.Count,
        }).ToList();
    }
}