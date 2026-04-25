using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scans;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scans")]
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