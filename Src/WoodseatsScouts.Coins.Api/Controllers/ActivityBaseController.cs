using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Activity Bases")]
[Route("api/activities/bases")]
public class ActivityBaseController( IAppDbContext appDbContext) : ControllerBase
{   
    [HttpGet]
    [Route("")]
    public ActionResult GetActivityBases()
    {
        return Ok(appDbContext.ActivityBases.Select(x => new ActivityBasesDto(x)));
    }
    
    [HttpGet]
    [Route("{activityBaseId:int}")]
    public ActionResult GetCoinCodesByActivityBase(int activityBaseId, bool showOnlyAvailable)
    {
        var coins = appDbContext.Coins.Where(x => x.ActivityBaseId == activityBaseId);
        if (showOnlyAvailable)
        {
            coins = coins.Where(x => !x.MemberId.HasValue);
        }
        
        return Ok(coins.Select(x => x.Code));
    }
}