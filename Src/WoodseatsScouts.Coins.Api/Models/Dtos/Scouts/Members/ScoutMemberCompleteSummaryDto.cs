using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scans;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberCompleteSummaryDto
{
    public ScoutMemberCompleteSummaryDto(ScoutMember scoutMember)
    {
        if (scoutMember.ScanSessions == null)
        {
            throw new InvalidOperationException("Scout member must load navigation property ScanSession");
        }
        
        var haulResults = scoutMember.ScanSessions.Select(scanSession =>
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
        HaulResults = haulResults;

        ScoutMemberCompleteSummaryStatsDto = new ScoutMemberCompleteSummaryStatsDto();

        var grouping = haulResults
            .SelectMany(x => x.ActivityBaseHaulResultDtos)
            .GroupBy(x => x.ActivityBaseName)
            .ToList();

        var maxCount = grouping.Any() ? grouping.Max(x => x.Count()) : 0;
        var minCount = grouping.Any() ? grouping.Min(x => x.Count()) : 0;

        ScoutMemberCompleteSummaryStatsDto.MostVisitedActivityBase = new ScoutMemberCompleteSummaryStatsActivityBaseInfoDto
        {
            Names = grouping.Where(x => x.Count() == maxCount).Select(x => x.Key).ToList(),
            TimesVisited = maxCount,
        };
        ScoutMemberCompleteSummaryStatsDto.LeastVisitedActivityBase = new ScoutMemberCompleteSummaryStatsActivityBaseInfoDto
        {
            Names = grouping.Where(x => x.Count() == minCount).Select(x => x.Key).ToList(),
            TimesVisited = minCount,
        };

        ScoutMemberCompleteSummaryStatsDto.MostScans = scoutMember.ScanSessions
            .Select(x => x.ScanCoins.Count)
            .DefaultIfEmpty(0)
            .Max() ;
        ScoutMemberCompleteSummaryStatsDto.TotalTokensScanned = scoutMember.ScanSessions
            .Select(x => x.ScanCoins.Count)
            .DefaultIfEmpty(0)
            .Min() ;
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