using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Abstractions;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AppStateController : ControllerBase
{
    private readonly IScoutsAppEnvironment scoutsAppEnvironment;

    public AppStateController(IScoutsAppEnvironment scoutsAppEnvironment)
    {
        this.scoutsAppEnvironment = scoutsAppEnvironment;
    }
    
    [HttpGet]
    public ActionResult Get()
    {
        return Ok(scoutsAppEnvironment.ScoutsAppMode.ToString());
    }
}