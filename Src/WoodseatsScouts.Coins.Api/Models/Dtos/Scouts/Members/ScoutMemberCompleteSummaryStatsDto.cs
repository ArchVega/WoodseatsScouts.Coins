namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberCompleteSummaryStatsDto
{
    public List<ScoutMemberCompleteSummaryStatsActivityBaseInfoDto> MostVisitedActivityBasesByParticipant { get; set; } = [];

    public List<ScoutMemberCompleteSummaryStatsActivityBaseInfoDto> LeastVisitedActivityBasesByParticipant { get; set; } = [];
    
    public List<ScoutMemberCompleteSummaryStatsActivityBaseInfoDto> LeastVisitedActivityBasesByOtherParticipants { get; set; } = [];

    public int MostScans { get; set; }

    public int TotalTokensScanned { get; set; }
}