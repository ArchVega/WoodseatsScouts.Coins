using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Models;

public class ReportViewModel
{
    public dynamic LastThreeUsersToScanPoints { get; set; }
    
    public List<GroupTotalPoints> TopThreeGroupsInLastHour { get; set; }
    
    public List<GroupTotalPoints> GroupsWithMostPointsThisWeekend { get; set; }
    
    public double SecondsUntilDeadline { get; set; }
    
    public string Title { get; set; }
    
    private static readonly long DatetimeMinTimeTicks =
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

}