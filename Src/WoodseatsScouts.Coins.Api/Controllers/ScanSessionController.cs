using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;

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
    
    [HttpDelete]
    [Route("{scanSessionId:int}")]
    public async Task<ActionResult> DeleteScanSession(int scanSessionId)
    {
        var rows = await appDbContext.ScanSessions
            .Where(m => m.Id == scanSessionId)
            .ExecuteDeleteAsync();
        
        return rows == 0 ? NotFound() : NoContent();
    }
}