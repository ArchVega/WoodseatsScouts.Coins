using System.Globalization;

namespace WoodseatsScouts.Coins.Api.Config;

// dotcover disable
public class AppSettings
{
    public static string ApiAuthenticationTokenKey = "X-Coins-Authentication-Token";
    
    public string AppVersion { get; set; }
    public string ContentRootDirectory { get; set; }

    public int MinutesToLockScavengedCoins { get; set; }
    
    public string AuthenticationToken { get; set; }
}