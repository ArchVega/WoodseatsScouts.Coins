namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class MemberCompleteSummaryStatsDto
{
    public MemberCompleteSummaryStatsActivityBaseInfoDto MostVisitedActivityBase { get; set; } = null!;

    public MemberCompleteSummaryStatsActivityBaseInfoDto LeastVisitedActivityBase { get; set; } = null!;

    public int MostScans { get; set; }

    public int TotalTokensScanned { get; set; }
}