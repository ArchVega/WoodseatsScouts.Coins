using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CoinsController : ControllerBase
{
    private readonly IAppDbContext appDbContext;

    public CoinsController(IAppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    [HttpGet]
    [Route("{code}/Scan/{memberCode}")]
    public IActionResult GetCoin(string code, string memberCode)
    {
        
        if (appDbContext.Members!.Any(x => x.Code == code))
        {
            return BadRequest("Expected coin code but received user code.");
        }

        CoinPointTranslationResult result;
        
        try
        {
            result = CodeTranslator.TranslateCoinPointCode(code);
        }
        catch (CodeTranslationException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"A coin with the code '{code}' could not be parsed.");
        }

        var dbCoin = appDbContext.Coins!.SingleOrDefault(x => x.Code == code);

        if (dbCoin == null)
        {
            return NotFound($"A coin with the code '{code}' was not found in the database.");
        }

        var member = appDbContext.Members!.Single(x => x.Code == memberCode);
        
        if (member.Id == dbCoin.MemberId)
        {
            return base.Conflict($"The coin has already been scavenged by {member.FirstName}");
        }

        if (dbCoin.MemberId.HasValue)
        {
            var memberWhoScavengedCoin = appDbContext.Members!.Single(x => x.Id == dbCoin.MemberId);
            return base.Conflict($"The coin with code '{code}' has already been scavenged by {memberWhoScavengedCoin.FullName}!");
        }

        dbCoin.MemberId = member.Id;
        appDbContext.SaveChanges();

        return Ok(new
        {
            result.PointValue,
            result.BaseNumber,
            Code = code
        });
    }
}