using System.Globalization;

namespace WoodseatsScouts.Coins.Api.Config;

// dotcover disable
public class AppSettings
{
    public string AppVersion { get; set; }
    public string ContentRootDirectory { get; set; }

    public int MinutesToLockScavengedCoins { get; set; }
    
    public int SecondsBetweenScavengerHauls { get; set; }
}