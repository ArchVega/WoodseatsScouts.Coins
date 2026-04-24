using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scan Sessions")]
[Route("api/scans/sessions")]
public class ScanSessionController(IAppDbContext appDbContext, IOptions<LeaderboardSettings> leaderboardSettingsOptions) : ControllerBase
{   
    [HttpGet]
    [Route("latest")]
    public ActionResult LatestScans()
    {
        var latestScans = appDbContext.GetLatestScans(leaderboardSettingsOptions.Value.NumberOfLatestScansToDisplay);

        return Ok(latestScans);
    }

    // todo: move to client side env file
    // // move
    // [HttpGet]
    // [Route("RefreshSecondsForLatestScans")]
    // public ActionResult RefreshSecondsForLatestScans()
    // {
    //     return Ok(leaderboardSettingsOptions.Value.Last6ScavengersPageRefreshSeconds);
    // }
}