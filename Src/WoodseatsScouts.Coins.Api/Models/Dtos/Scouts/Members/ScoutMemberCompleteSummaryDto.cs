using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scans;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberCompleteSummaryDto
{
    public ScoutMemberCompleteSummaryDto()
    {
    }
    
    public ScoutMemberCompleteSummaryDto(ScoutMember scoutMember)
    {
        if (scoutMember.ScanSessions == null)
        {
            throw new InvalidOperationException("Scout member must load navigation property ScanSession");
        }

        var haulResultDtos = scoutMember.ScanSessions.Select(scanSession =>
        {
            var groupedByActivityBase = scanSession.ScanCoins.GroupBy(x => x.Coin!.ActivityBaseId).ToList();

            var activityBaseResults = groupedByActivityBase.Select(x =>
            {
                return new ActivityBaseHaulResultDto
                {
                    ActivityBaseId = x.Key,
                    ActivityBaseName = x.ElementAt(0).Coin!.ActivityBase!.Name,
                    TotalPoints = x.Sum(y => y.CalculatedEffectivePoints!.Value),
                    CoinsScanned = x.Count(),
                    ScannedCoinDtos = x.Select(y => new ScannedCoinDto(y)).ToList()
                };
            }).ToList();

            return new HaulResultDto
            {
                ScanSessionId = scanSession.Id,
                HauledAtIso8601 = scanSession.CompletedAt.ToUniversalTime().ToString("o"),
                TotalPoints = (int)scanSession.ScanCoins.Sum(x => x.CalculatedEffectivePoints)!,
                ActivityBaseHaulResultDtos = activityBaseResults
            };
        }).ToList();

        var cacheBuster = DateTime.UtcNow.Ticks;

        Id = scoutMember.Id;
        ScoutMemberCode = scoutMember.Code;
        HasImage = scoutMember.HasImage;
        ComputedImagePath = scoutMember.HasImage ? $"scouts/members/{scoutMember.Id}/photo?{cacheBuster}" : "scouts/members/photo/placeholder";
        ScoutMemberNumber = scoutMember.Number;
        FirstName = scoutMember.FirstName;
        LastName = scoutMember.LastName;
        FullName = scoutMember.FullName;
        ScoutGroupId = scoutMember.ScoutGroup.Id;
        ScoutGroupName = scoutMember.ScoutGroup.Name;
        ScoutSectionCode = scoutMember.ScoutSectionCode;
        ScoutSectionName = scoutMember.ScoutSection.Name;
        TotalPoints = (int)scoutMember.ScanSessions.SelectMany(y => y.ScanCoins.Select(z => z.CalculatedEffectivePoints)).Sum()!;
        HaulResults = haulResultDtos;

        ScoutMemberCompleteSummaryStatsDto = new ScoutMemberCompleteSummaryStatsDto();

        ScoutMemberCompleteSummaryStatsDto.MostScans = scoutMember.ScanSessions
            .Select(x => x.ScanCoins.Count)
            .DefaultIfEmpty(0)
            .Max();
        ScoutMemberCompleteSummaryStatsDto.TotalTokensScanned = scoutMember.ScanSessions
            .Select(x => x.ScanCoins.Count)
            .DefaultIfEmpty(0)
            .Sum();

        var mostVisited = haulResultDtos
            .SelectMany(x => x.ActivityBaseHaulResultDtos)
            .GroupBy(activityBase => activityBase.ActivityBaseName)
            .Select(g => new
            {
                ActivityBaseName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(3)
            .ToList();

        ScoutMemberCompleteSummaryStatsDto.MostVisitedActivityBasesByParticipant
            = mostVisited.Select(x => new ScoutMemberCompleteSummaryStatsActivityBaseInfoDto
            {
                Name = x.ActivityBaseName,
                TimesVisited = x.Count,
            }).ToList();
        
       // least by participant and by others done in the controller.
    }

    public int Id { get; set; }

    public string FullName { get; set; }

    public string ScoutMemberCode { get; set; }

    public bool HasImage { get; set; }

    public string ComputedImagePath { get; set; }

    public int ScoutMemberNumber { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public int ScoutGroupId { get; init; }

    public string ScoutGroupName { get; set; }

    public string ScoutSectionCode { get; set; }

    public string ScoutSectionName { get; set; }

    public int TotalPoints { get; set; }

    public DateTime? LatestCompletedAtTime { get; set; }

    public List<HaulResultDto> HaulResults { get; set; }

    public ScoutMemberCompleteSummaryStatsDto ScoutMemberCompleteSummaryStatsDto { get; set; }
}