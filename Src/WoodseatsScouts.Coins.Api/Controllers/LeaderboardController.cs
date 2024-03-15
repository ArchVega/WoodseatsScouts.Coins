using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LeaderboardController(IAppDbContext appDbContext, IOptions<LeaderboardSettings> leaderboardSettingsOptions)
    : ControllerBase
{
    private readonly LeaderboardSettings leaderboardSettings = leaderboardSettingsOptions.Value;

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

        var secondsUntilDeadline = leaderboardSettings.ScavengerHuntDeadline > DateTime.Now
            ? (leaderboardSettings.ScavengerHuntDeadline - DateTime.Now).TotalSeconds
            : 0;

        var reportViewModel = new LeaderboardViewModel
        {
            Title = leaderboardSettings.PageTitle,
            SecondsUntilDeadline = secondsUntilDeadline,
            ReportRefreshSeconds = leaderboardSettings.PageRefreshSeconds,
            LastThreeUsersToScanPoints = top3MembersWithPointsAttached,
            TopThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour(),
            GroupsWithMostPointsThisWeekend = appDbContext.GetGroupsWithMostPoints()
        };

        return reportViewModel;
    }
}