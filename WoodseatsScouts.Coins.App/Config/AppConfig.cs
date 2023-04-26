using System.Globalization;

namespace WoodseatsScouts.Coins.App.Config;

public class AppConfig
{
    public AppConfig(IConfiguration configuration)
    {
        ReportDeadline = DateTime.Parse(configuration["ReportDeadline"], new CultureInfo("en-GB"));
        ReportTitle = configuration["ReportTitle"];
        ReportRefreshSeconds = Convert.ToInt32(configuration["ReportRefreshSeconds"]);
        ContentRootDirectory = configuration["ContentRootDirectory"];
    }

    public DateTime ReportDeadline { get; }
    
    public string ReportTitle { get; set; }
    
    public int ReportRefreshSeconds { get; set; }

    public string? ContentRootDirectory { get; set; }
}