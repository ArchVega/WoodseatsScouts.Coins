// dotcover disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AppStateController(IScoutsAppEnvironment scoutsAppEnvironment, IOptions<AppSettings> appSettingsOptions) : ControllerBase
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
}