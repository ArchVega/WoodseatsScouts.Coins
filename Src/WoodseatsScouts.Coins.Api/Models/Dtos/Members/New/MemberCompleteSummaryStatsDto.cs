namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

public class MemberCompleteSummaryStatsDto
{
    public MemberCompleteSummaryStatsActivityBaseInfoDto MostVisitedActivityBase { get; set; }
    
    public MemberCompleteSummaryStatsActivityBaseInfoDto LeastVisitedActivityBase { get; set; }

    public int MostScans { get; set; }

    public int TotalTokensScanned { get; set; }
}

public class MemberCompleteSummaryStatsActivityBaseInfoDto
{
    public List<string> Names { get; set; }

    public int TimesVisited { get; set; }

}