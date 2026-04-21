using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;
using WoodseatsScouts.Coins.Api.Models.View.Members;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

public class MemberCompleteSummaryDto
{
    public MemberCompleteSummaryDto(Member member)
    {
        var haulResults = member.ScavengeResults.Select(scavengeResult =>
        {
            var groupedByActivityBase = scavengeResult.ScavengedCoins.GroupBy(x => x.Coin.ActivityBaseId).ToList();

            var activityBaseResults = groupedByActivityBase.Select(x =>
            {
                return new ActivityBaseHaulResultDto
                {
                    ActivityBaseId = x.Key,
                    ActivityBaseName = x.ElementAt(0).Coin.ActivityBase.Name,
                    TotalPoints = x.Sum(y => y.Coin.Value),
                    CoinsScanned = x.Count(),
                    Coins = x.Select(y => new CoinDto(y.Coin.Value, y.Coin.ActivityBase.Id, y.Coin.Code)).ToList()
                };
            }).ToList();

            return new HaulResultDto
            {
                ScavengerResultId = scavengeResult.Id,
                HauledAtIso8601 = scavengeResult.CompletedAt.ToUniversalTime().ToString("o"), // ISO 8601
                TotalPoints = scavengeResult.ScavengedCoins.Sum(x => x.Coin.Value), // changed
                ActivityBaseHaulResultDtos = activityBaseResults
            };
        }).ToList();

        var cacheBuster = DateTime.UtcNow.Ticks;

        Id = member.Id;
        MemberCode = member.Code;
        HasImage = member.HasImage;
        ComputedImagePath = member.HasImage ? $"Members/{member.Id}/Photo?{cacheBuster}" : "Members/Photo/Placeholder";
        MemberNumber = member.Number;
        FirstName = member.FirstName;
        LastName = member.LastName;
        FullName = member.FullName;
        ScoutGroupId = member.ScoutGroup.Id;
        ScoutGroupName = member.ScoutGroup.Name;
        SectionId = member.SectionId;
        SectionName = member.Section.Name;
        // changed
        TotalPoints = member.ScavengeResults.SelectMany(y => y.ScavengedCoins.Select(z => z.Coin.Value)).Sum();
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

        MemberCompleteSummaryStatsDto.MostScans = member.ScavengeResults
            .Select(x => x.ScavengedCoins.Count)
            .DefaultIfEmpty(0)
            .Max() ;
        MemberCompleteSummaryStatsDto.TotalTokensScanned = member.ScavengeResults
            .Select(x => x.ScavengedCoins.Count)
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

    public string SectionId { get; set; } // Todo Section is now "SectionId". Rename

    public string SectionName { get; set; }

    public int TotalPoints { get; set; }

    public DateTime? LatestCompletedAtTime { get; set; }

    public List<HaulResultDto> HaulResults { get; set; }

    public MemberCompleteSummaryStatsDto MemberCompleteSummaryStatsDto { get; set; }
}