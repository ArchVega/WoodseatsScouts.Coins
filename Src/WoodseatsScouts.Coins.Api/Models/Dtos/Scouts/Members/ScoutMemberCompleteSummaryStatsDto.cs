namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberCompleteSummaryStatsDto
{
    public List<ScoutMemberCompleteSummaryStatsActivityBaseInfoDto> MostVisitedActivityBases { get; set; } = [];

    public List<ScoutMemberCompleteSummaryStatsActivityBaseInfoDto> LeastVisitedActivityBases { get; set; } = [];

    public int MostScans { get; set; }

    public int TotalTokensScanned { get; set; }
}