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
}