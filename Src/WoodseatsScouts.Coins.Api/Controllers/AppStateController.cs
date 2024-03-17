// dotcover disable
using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Abstractions;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AppStateController(IScoutsAppEnvironment scoutsAppEnvironment) : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Ok(scoutsAppEnvironment.ScoutsAppMode.ToString());
    }
}