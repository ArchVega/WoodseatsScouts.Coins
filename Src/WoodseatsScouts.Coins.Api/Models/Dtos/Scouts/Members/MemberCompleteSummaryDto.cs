using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class MemberCompleteSummaryDto
{
    public MemberCompleteSummaryDto(ScoutMember scoutMember)
    {
        var haulResults = scoutMember.ScanSessions.Select(scavengeResult =>
        {
            var groupedByActivityBase = scavengeResult.ScanCoins.GroupBy(x => x.Coin!.ActivityBaseId).ToList();

            var activityBaseResults = groupedByActivityBase.Select(x =>
            {
                return new ActivityBaseHaulResultDto
                {
                    ActivityBaseId = x.Key,
                    ActivityBaseName = x.ElementAt(0).Coin!.ActivityBase!.Name,
                    TotalPoints = x.Sum(y => y.Coin!.Value),
                    CoinsScanned = x.Count(),
                    Coins = x.Select(y => new CoinDto(y.Coin!.Value, y.Coin!.ActivityBase!.Id, y.Coin.Code)).ToList()
                };
            }).ToList();

            return new HaulResultDto
            {
                ScavengerResultId = scavengeResult.Id,
                HauledAtIso8601 = scavengeResult.CompletedAt.ToUniversalTime().ToString("o"),
                TotalPoints = scavengeResult.ScanCoins.Sum(x => x.Coin!.Value),
                ActivityBaseHaulResultDtos = activityBaseResults
            };
        }).ToList();

        var cacheBuster = DateTime.UtcNow.Ticks;

        Id = scoutMember.Id;
        MemberCode = scoutMember.Code;
        HasImage = scoutMember.HasImage;
        ComputedImagePath = scoutMember.HasImage ? $"scouts/members/{scoutMember.Id}/photo?{cacheBuster}" : "scouts/members/photo/placeholder";
        MemberNumber = scoutMember.Number;
        FirstName = scoutMember.FirstName;
        LastName = scoutMember.LastName;
        FullName = scoutMember.FullName;
        ScoutGroupId = scoutMember.ScoutGroup.Id;
        ScoutGroupName = scoutMember.ScoutGroup.Name;
        SectionCode = scoutMember.ScoutSectionId;
        SectionName = scoutMember.ScoutSection.Name;
        TotalPoints = scoutMember.ScanSessions.SelectMany(y => y.ScanCoins.Select(z => z.Coin!.Value)).Sum();
        HaulResults = haulResults;

        MemberCompleteSummaryStatsDto = new MemberCompleteSummaryStatsDto();

        var grouping = haulResults
            .SelectMany(x => x.ActivityBaseHaulResultDtos)
            .GroupBy(x => x.ActivityBaseName)
            .ToList();

        var maxCount = grouping.Any() ? grouping.Max(x => x.Count()) : 0;
        var minCount = grouping.Any() ? grouping.Min(x => x.Count()) : 0;

        MemberCompleteSummaryStatsDto.MostVisitedActivityBase = new MemberCompleteSummaryStatsActivityBaseInfoDto
        {
            Names = grouping.Where(x => x.Count() == maxCount).Select(x => x.Key).ToList(),
            TimesVisited = maxCount,
        };
        MemberCompleteSummaryStatsDto.LeastVisitedActivityBase = new MemberCompleteSummaryStatsActivityBaseInfoDto
        {
            Names = grouping.Where(x => x.Count() == minCount).Select(x => x.Key).ToList(),
            TimesVisited = minCount,
        };

        MemberCompleteSummaryStatsDto.MostScans = scoutMember.ScanSessions
            .Select(x => x.ScanCoins.Count)
            .DefaultIfEmpty(0)
            .Max() ;
        MemberCompleteSummaryStatsDto.TotalTokensScanned = scoutMember.ScanSessions
            .Select(x => x.ScanCoins.Count)
            .DefaultIfEmpty(0)
            .Min() ;
    }

    public int Id { get; set; }

    public string FullName { get; set; }

    public string MemberCode { get; set; }

    public bool HasImage { get; set; }

    public string ComputedImagePath { get; set; }

    public int MemberNumber { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public int ScoutGroupId { get; init; }

    public string ScoutGroupName { get; set; }

    public string SectionCode { get; set; }

    public string SectionName { get; set; }

    public int TotalPoints { get; set; }

    public DateTime? LatestCompletedAtTime { get; set; }

    public List<HaulResultDto> HaulResults { get; set; }

    public MemberCompleteSummaryStatsDto MemberCompleteSummaryStatsDto { get; set; }
}