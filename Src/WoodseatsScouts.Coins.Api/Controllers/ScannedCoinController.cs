using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scans;
using WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scans")]
[Route("api/scans/coins")]
public class ScannedCoinController(IAppDbContext appDbContext, IOptions<AppSettings> appSettingsOptions) : ControllerBase
{
    [HttpGet]
    [Route("")]
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
    [Route("{scannedCoinId:int}")]
    public ActionResult GetScannedCoin(int scannedCoinId)
    {
        // todo: use projection
        var scannedCoin = appDbContext
            .ScannedCoins
            .Include(x => x.Coin)
            .Single(x => x.Id == scannedCoinId);

        return Ok(new ScannedCoinDto(scannedCoin));
    }
    
    [HttpPut]
    [Route("{scannedCoinId:int}")]
    public ActionResult UpdateScannedCoinPoints(int scannedCoinId, [FromBody] UpdateScannedCoinPointsValueRequest request)
    {
        var scannedCoin = appDbContext
            .ScannedCoins
            .Single(x => x.Id == scannedCoinId);

        scannedCoin.PointsOverride = request.NewPointsValue;

        appDbContext.SaveChanges();
        
        var updatedScannedCoin = appDbContext
            .ScannedCoins
            .Include(x => x.Coin)
            .ThenInclude(x => x.ActivityBase)
            .Single(x => x.Id == scannedCoinId);
        
        return Ok(new ScannedCoinDto(updatedScannedCoin));
    }

    [HttpDelete]
    [Route("{scannedCoinId:int}")]
    public async Task<ActionResult> DeleteScannedCoin(int scannedCoinId)
    {
        var rows = await appDbContext.ScannedCoins
            .Where(m => m.Id == scannedCoinId)
            .ExecuteDeleteAsync();
        
        return rows == 0 ? NotFound() : NoContent();
    }
}