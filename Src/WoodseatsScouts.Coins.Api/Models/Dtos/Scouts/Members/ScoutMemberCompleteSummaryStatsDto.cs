namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberCompleteSummaryStatsDto
{
    public ScoutMemberCompleteSummaryStatsActivityBaseInfoDto MostVisitedActivityBase { get; set; } = null!;

    public ScoutMemberCompleteSummaryStatsActivityBaseInfoDto LeastVisitedActivityBase { get; set; } = null!;

    public int MostScans { get; set; }

    public int TotalTokensScanned { get; set; }
}