// dotcover disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Application Info")]
[Route("api/application")]
public class ApplicationInfoController(IScoutsAppEnvironment scoutsAppEnvironment, IOptions<AppSettings> appSettingsOptions) : ControllerBase
{
    [HttpGet]
    [Route("mode")]
    public ActionResult Get()
    {
        return Ok(scoutsAppEnvironment.ScoutsAppMode.ToString());
    }
    
    [HttpGet]
    [Route("app-version")]
    public ActionResult AppVersion()
    {
        return new OkObjectResult(appSettingsOptions.Value.AppVersion);
    }
}