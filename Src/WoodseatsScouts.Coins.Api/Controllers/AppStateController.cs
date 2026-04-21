// dotcover disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppStateController(IAppDbContext appDbContext, IScoutsAppEnvironment scoutsAppEnvironment, IOptions<AppSettings> appSettingsOptions) : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Ok(scoutsAppEnvironment.ScoutsAppMode.ToString());
    }
    
    [HttpGet]
    [Route("AppVersion")]
    public ActionResult AppVersion()
    {
        return new OkObjectResult(appSettingsOptions.Value.AppVersion);
    }
    
    // move these to controller, ReferenceDataController?
    [HttpGet]
    [Route("ScoutGroups")]
    public ActionResult ScoutGroups()
    {
        return Ok(appDbContext.ScoutGroups.Select(x => new ScoutGroupDto(x)));
    }
    
    // move these to controller, ReferenceDataController?
    [HttpGet]
    [Route("Sections")]
    public ActionResult Sections()
    {
        return Ok(appDbContext.Sections.Select(x => new SectionDto(x)));
    }
}