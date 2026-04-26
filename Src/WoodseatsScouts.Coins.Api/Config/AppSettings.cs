using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace WoodseatsScouts.Coins.Api.Config;

// dotcover disable
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class AppSettings
{
    public const string ApiAuthenticationTokenKey = "X-Coins-Authentication-Token";

    public required string AppVersion { get; set; }
    
    public required string ContentRootDirectory { get; init; }

    public int MinutesToLockScavengedCoins { get; set; }
    
    public int LoginPauseDurationSeconds { get; set; }

    public required string ParticipantPlaceholderImagePath { get; set; }
    
    public int NumberOfLatestScansToDisplay { get; set; }
}