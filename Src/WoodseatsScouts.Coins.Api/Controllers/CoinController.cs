using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("api/coins")]
public class CoinController(
    IAppDbContext appDbContext,
    SystemDateTimeProvider systemDateTimeProvider) : ControllerBase
{
    [HttpGet]
    [Route("{code}/scans/{memberCode}")]
    public IActionResult GetCoin(string code, string memberCode)
    {
        var result = CodeTranslator.TranslateCoinCode(code);

        var dbCoin = appDbContext.Coins!.SingleOrDefault(x => x.Code == code);

        if (dbCoin == null)
        {
            return NotFound($"A coin with the code '{code}' was not found in the database.");
        }

        var member = appDbContext.ScoutMembers!.SingleOrDefault(x => x.Code == memberCode);

        if (member == null)
        {
            return NotFound($"A member with the code '{code}' was not found in the database.");
        }

        if (member.Id == dbCoin.MemberId && systemDateTimeProvider.Now < dbCoin.LockUntil)
        {
            return base.Conflict($"The coin has already been scavenged by {member.FirstName}");
        }

        // ReSharper disable once InvertIf
        if (dbCoin.MemberId.HasValue && systemDateTimeProvider.Now < dbCoin.LockUntil)
        {
            var memberWhoScavengedCoin = appDbContext.ScoutMembers!.Single(x => x.Id == dbCoin.MemberId);
            
            var message = $"This points token has already been used by {memberWhoScavengedCoin.FullName}. Please hand it to a District Camp Leader";
            return base.Conflict(message);
        }

        /*  The coin has either never been scavenged, or it has but enough time has passed to allow it to be scavenged again.
            These properties will be set in the AddPointsToMember method. */
        dbCoin.MemberId = null;
        dbCoin.LockUntil = null;
        appDbContext.SaveChanges();

        return Ok(new CoinDto(result.PointValue, result.ActivityBaseId, code));
    }
    
    [HttpPost]
    [Route("")]
    public ActionResult CreateCoins([FromBody] CreateCoinDto createCoinDto)
    {
        if (createCoinDto.ActivityBaseId.HasValue && !string.IsNullOrWhiteSpace(createCoinDto.ActivityBaseName))
        {
            return BadRequest("Both BaseId and BaseName were provided. Provide one.");
        }

        if (!createCoinDto.ActivityBaseId.HasValue && string.IsNullOrWhiteSpace(createCoinDto.ActivityBaseName))
        {
            return BadRequest("Either BaseId or BaseName is required.");
        }

        if (createCoinDto.PointsPerCoin == 0)
        {
            return BadRequest("Points must be provided.");
        }

        var baseId = string.IsNullOrWhiteSpace(createCoinDto.ActivityBaseName)
            ? createCoinDto.ActivityBaseId!.Value
            : appDbContext.ActivityBases!.Single(x => x.Name == createCoinDto.ActivityBaseName).Id!;

        var coins = appDbContext.CreateCoins(baseId!, createCoinDto.PointsPerCoin, createCoinDto.NumberToCreate);

        return Ok(coins);
    }
}