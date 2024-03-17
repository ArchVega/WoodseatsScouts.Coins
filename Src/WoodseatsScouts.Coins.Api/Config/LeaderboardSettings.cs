namespace WoodseatsScouts.Coins.Api.Config;

// dotcover disable
public class LeaderboardSettings
{
    public DateTime ScavengerHuntStartTime { get; set; }
    
    public DateTime ScavengerHuntDeadline { get; set; }
    
    public string PageTitle { get; set; }
    
    public int PageRefreshSeconds { get; set; }

}