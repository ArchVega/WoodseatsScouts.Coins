using System.Globalization;

namespace WoodseatsScouts.Coins.App.Config;

public class AppConfig
{
    public AppConfig(IConfiguration configuration)
    {
        ReportDeadline = DateTime.Parse(configuration["ReportDeadline"], new CultureInfo("en-GB"));
        ReportTitle = configuration["ReportTitle"];
    }

    public DateTime ReportDeadline { get; }
    
    public string ReportTitle { get; set; }
}