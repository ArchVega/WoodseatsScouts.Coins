using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scan Sessions")]
[Route("api/scans/sessions")]
public class ScanSessionController(IAppDbContext appDbContext, IOptions<AppSettings> appSettingsOptions) : ControllerBase
{   
    [HttpGet]
    [Route("latest")]
    public ActionResult LatestScans()
    {
        var latestScans = appDbContext.GetLatestScans(appSettingsOptions.Value.NumberOfLatestScansToDisplay);

        return Ok(latestScans);
    }
}