using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scout Sections")]
[Route("api/scouts/sections")]
public class ScoutSectionController( IAppDbContext appDbContext) : ControllerBase
{   
    [HttpGet]
    [Route("")]
    public ActionResult ScoutSections()
    {
        return Ok(appDbContext.ScoutSections!.Select(x => new ScoutSectionDto(x)));
    }
}