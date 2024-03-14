using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly IAppDbContext appDbContext;
    private readonly AppConfig appConfig;

    public LeaderboardController(IAppDbContext appDbContext, AppConfig appConfig)
    {
        this.appDbContext = appDbContext;
        this.appConfig = appConfig;
    }
    
    [HttpGet]
    [Route("Report")]
    public LeaderboardViewModel Report()
    {
        var top3MembersWithPointsAttached = appDbContext.GetLastThreeUsersToScanPoints()
            .Select(x => new
            {
                x.Id,
                MemberCode = x.Code,
                x.HasImage,
                MemberNumber = x.Number,
                x.FirstName,
                x.LastName,
                TroopName = x.Troop.Name,
                Section = x.SectionId,
                SectionName = x.Section.Name,
                TotalPoints = x.ScavengeResults.Last().ScavengedCoins.Sum(y => y.PointValue)
            });

        var secondsUntilDeadline = appConfig.ReportDeadline > DateTime.Now
            ? (appConfig.ReportDeadline - DateTime.Now).TotalSeconds
            : 0;

        var reportViewModel = new LeaderboardViewModel
        {
            Title = appConfig.ReportTitle,
            SecondsUntilDeadline = secondsUntilDeadline,
            ReportRefreshSeconds = appConfig.ReportRefreshSeconds,
            LastThreeUsersToScanPoints = top3MembersWithPointsAttached,
            TopThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour(),
            GroupsWithMostPointsThisWeekend = appDbContext.GetGroupsWithMostPointsThisWeekend()
        };

        return reportViewModel;
    }
}