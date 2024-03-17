using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CoinsController(IAppDbContext appDbContext) : ControllerBase
{
    [HttpGet]
    [Route("{code}/Scan/{memberCode}")]
    public IActionResult GetCoin(string code, string memberCode)
    {
        var result = CodeTranslator.TranslateCoinCode(code);

        var dbCoin = appDbContext.Coins!.SingleOrDefault(x => x.Code == code);

        if (dbCoin == null)
        {
            return NotFound($"A coin with the code '{code}' was not found in the database.");
        }

        var member = appDbContext.Members!.SingleOrDefault(x => x.Code == memberCode);

        if (member == null)
        {
            return NotFound($"A member with the code '{code}' was not found in the database.");
        }

        if (member.Id == dbCoin.MemberId)
        {
            return base.Conflict($"The coin has already been scavenged by {member.FirstName}");
        }

        // ReSharper disable once InvertIf
        if (dbCoin.MemberId.HasValue)
        {
            var memberWhoScavengedCoin = appDbContext.Members!.Single(x => x.Id == dbCoin.MemberId);
            return base.Conflict($"The coin with code '{code}' has already been scavenged by {memberWhoScavengedCoin.FullName}!");
        }

        return Ok(new CoinViewModel(result.PointValue, result.BaseNumber, code));
    }
}