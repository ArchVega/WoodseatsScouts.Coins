using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Api.Models.Requests.Coins;
using WoodseatsScouts.Coins.Api.Services;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("api/coins")]
public class CoinController(IAppDbContext appDbContext, ICoinService coinService) : ControllerBase
{
    /// <summary>
    /// Get all coins.
    /// </summary>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await coinService.GetCoinFullDtos());
    }
    
    /// <summary>
    /// Create new coins.
    /// </summary>
    [HttpPost]
    [Route("")]
    public ActionResult CreateCoins([FromBody] CreateCoinsRequest createCoinsRequest)
    {
        if (createCoinsRequest.ActivityBaseId.HasValue && !string.IsNullOrWhiteSpace(createCoinsRequest.ActivityBaseName))
        {
            return BadRequest("Both BaseId and BaseName were provided. Provide one.");
        }

        if (!createCoinsRequest.ActivityBaseId.HasValue && string.IsNullOrWhiteSpace(createCoinsRequest.ActivityBaseName))
        {
            return BadRequest("Either BaseId or BaseName is required.");
        }

        if (createCoinsRequest.PointsPerCoin == 0)
        {
            return BadRequest("Points must be provided.");
        }

        var baseId = string.IsNullOrWhiteSpace(createCoinsRequest.ActivityBaseName)
            ? createCoinsRequest.ActivityBaseId!.Value
            : appDbContext.ActivityBases!.Single(x => x.Name == createCoinsRequest.ActivityBaseName).Id!;

        var coins = appDbContext.CreateCoins(baseId!, createCoinsRequest.PointsPerCoin, createCoinsRequest.NumberToCreate);

        return Ok(coins);
    }

    /// <summary>
    /// Assigns a coin to a scout member. 
    /// </summary>
    [HttpPut]
    [Route("{coinCode}/assign/{scoutMemberCode}")]
    public IActionResult AssignCoinToScoutMember(string coinCode, string scoutMemberCode)
    {
        var result = CodeTranslator.TranslateCoinCode(coinCode);

        var dbCoin = appDbContext.Coins!.SingleOrDefault(x => x.Code == coinCode);

        if (dbCoin == null)
        {
            return NotFound($"A coin with the code '{coinCode}' was not found in the database.");
        }

        var member = appDbContext.ScoutMembers!.SingleOrDefault(x => x.Code == scoutMemberCode);

        if (member == null)
        {
            return NotFound($"A member with the code '{coinCode}' was not found in the database.");
        }

        if (member.Id == dbCoin.MemberId && DateTime.UtcNow < dbCoin.LockUntil)
        {
            return base.Conflict($"The coin has already been scavenged by {member.FirstName}");
        }

        // ReSharper disable once InvertIf
        if (dbCoin.MemberId.HasValue && DateTime.UtcNow < dbCoin.LockUntil)
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

        return Ok(new CoinDto(result.PointValue, result.ActivityBaseId, coinCode));
    }
}