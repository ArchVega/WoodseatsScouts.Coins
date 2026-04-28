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

        var visitsPerSessionBaseByMember = appDbContext
            .ScoutMembers
            .Where(sm => sm.Id == scoutMemberId)
            .SelectMany(sm => sm.ScanSessions)
            .SelectMany(ss => ss.ScanCoins
                .Where(sc => sc.Coin != null && sc.Coin.ActivityBase != null)
                .Select(sc => new
                {
                    ActivityBaseId = sc.Coin.ActivityBase.Id,
                    SessionId = ss.Id
                }))
            .Distinct();
        
        var leastVisitedByParticipant = appDbContext
            .ActivityBases
            .GroupJoin(
                visitsPerSessionBaseByMember,
                ab => ab.Id,
                v => v.ActivityBaseId,
                (ab, visits) => new ScoutMemberCompleteSummaryStatsActivityBaseInfoDto
                {
                    Name = ab.Name,
                    TimesVisited = visits.Count()
                })
            .OrderBy(x => x.TimesVisited)
            .Take(3)
            .ToList();
        
        scoutMemberCompleteSummaryDto.ScoutMemberCompleteSummaryStatsDto.LeastVisitedActivityBasesByParticipant = leastVisitedByParticipant;
        
        var visitsPerSessionBaseByOthers = appDbContext
            .ScoutMembers
            .Where(sm => sm.Id != scoutMemberId)
            .SelectMany(sm => sm.ScanSessions)
            .SelectMany(ss => ss.ScanCoins
                .Where(sc => sc.Coin != null && sc.Coin.ActivityBase != null)
                .Select(sc => new
                {
                    ActivityBaseId = sc.Coin.ActivityBase.Id,
                    SessionId = ss.Id
                }))
            .Distinct();
        
        var leastVisitedByOtherParticipants = appDbContext
            .ActivityBases
            .GroupJoin(
                visitsPerSessionBaseByOthers,
                ab => ab.Id,
                v => v.ActivityBaseId,
                (ab, visits) => new ScoutMemberCompleteSummaryStatsActivityBaseInfoDto
                {
                    Name = ab.Name,
                    TimesVisited = visits.Count()
                })
            .OrderBy(x => x.TimesVisited)
            .Take(3)
            .ToList();
        
        scoutMemberCompleteSummaryDto.ScoutMemberCompleteSummaryStatsDto.LeastVisitedActivityBasesByOtherParticipants = leastVisitedByOtherParticipants;

        return scoutMemberCompleteSummaryDto;
    }
}