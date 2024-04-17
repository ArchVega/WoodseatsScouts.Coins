using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.View;

public class LeaderboardViewModel
{
    public dynamic LastThreeUsersToScanPoints { get; set; }
    
    public List<GroupPoints> TopThreeGroupsInLastHour { get; set; }
    
    public List<GroupPoints> GroupsWithMostPointsThisWeekend { get; set; }
    
    public double SecondsUntilDeadline { get; set; }
    
    public string Title { get; set; }
    
    public int ReportRefreshSeconds { get; set; }

    // dotcover disable
    private static readonly long DatetimeMinTimeTicks =
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

}