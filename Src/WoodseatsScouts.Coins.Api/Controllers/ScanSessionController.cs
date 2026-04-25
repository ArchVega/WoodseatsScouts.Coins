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
[Route("api/scans")]
public class ScanSessionController(IAppDbContext appDbContext, IOptions<AppSettings> appSettingsOptions) : ControllerBase
{
    [HttpGet]
    [Route("sessions/latest")]
    public ActionResult LatestScans()
    {
        var latestScans = appDbContext.GetLatestScans(appSettingsOptions.Value.NumberOfLatestScansToDisplay);

        return Ok(latestScans);
    }

    [HttpGet]
    [Route("coins")]
    public ActionResult GetScannedCoins()
    {
        // todo: projection
        var scannedCoinDtos = appDbContext
            .ScannedCoins
            .Include(x => x.Coin)
            .Select(x => new ScannedCoinDto(x)).ToList();

        return Ok(scannedCoinDtos);
    }

    [HttpGet]
    [Route("coins/{scannedCoinId:int}")]
    public ActionResult GetScannedCoin(int scannedCoinId)
    {
        // todo: use projection
        var scannedCoin = appDbContext
            .ScannedCoins
            .Include(x => x.Coin)
            .Single(x => x.Id == scannedCoinId);

        return Ok(new ScannedCoinDto(scannedCoin));
    }

    [HttpDelete]
    [Route("coins/{scannedCoinId:int}")]
    public async Task<ActionResult> DeleteScannedCoin(int scannedCoinId)
    {
        var rows = await appDbContext.ScannedCoins
            .Where(m => m.Id == scannedCoinId)
            .ExecuteDeleteAsync();
        
        return rows == 0 ? NotFound() : NoContent();
    }
}