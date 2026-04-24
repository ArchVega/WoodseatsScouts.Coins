using System.Globalization;

namespace WoodseatsScouts.Coins.Api.Config;

// dotcover disable
public class AppSettings
{
    public static string ApiAuthenticationTokenKey = "X-Coins-Authentication-Token";
    
    public string AppVersion { get; set; }
    public string ContentRootDirectory { get; set; }

    public int MinutesToLockScavengedCoins { get; set; }
    
    // todo: ?
    public string AuthenticationToken { get; set; }
    
    public int LoginPauseDurationSeconds { get; set; }

    public string ParticipantPlaceholderImagePath { get; set; }
    
    public int NumberOfLatestScansToDisplay { get; set; }
}