namespace WoodseatsScouts.Coins.Api.Config;

// dotcover disable
public class LeaderboardSettings
{
    public DateTime ScavengerHuntDeadline { get; set; }
    
    public string LeaderboardTitle { get; set; }
    
    public int LeaderboardRefreshSeconds { get; set; }

}