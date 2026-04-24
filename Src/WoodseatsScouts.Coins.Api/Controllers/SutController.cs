// dotcover disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("System Testing")]
[Route("api/system/tests")]
public class SutController(
    IAppDbContext appDbContext,
    IScoutsAppEnvironment scoutsAppEnvironment, 
    SystemDateTimeProvider systemDateTimeProvider) : ControllerBase
{
    [HttpGet]
    [Route("members")]
    public List<ScoutMember> GetMembers()
    {
        return appDbContext.ScoutMembers!.ToList();
    }

    [HttpPut]
    [Route("members/has-image/true")]
    public IActionResult SetAllMemberHasImagePropertyToTrue()
    {
        var members = appDbContext.ScoutMembers!.ToList();
        foreach (var member in members)
        {
            member.HasImage = true;
        }

        appDbContext.SaveChanges();

        return Ok("Updated all members HasImage property to true");
    }
    
    [HttpPut]
    [Route("datetime/{minutesToAdd:int?}")]
    public IActionResult SetSystemDateTime(int? minutesToAdd)
    {
        if (minutesToAdd.HasValue)
        {
            systemDateTimeProvider.SetDateTimeToSetTime(DateTime.UtcNow.AddMinutes(minutesToAdd.Value));
        }
        else
        {
            systemDateTimeProvider.SetDateTimeToSystemClock();
        }

        return Ok($"System datetime set to '{systemDateTimeProvider.Now}'");
    }
    
    [HttpGet]
    [Route("coins")]
    public ActionResult GetAll()
    {
        return Ok(appDbContext.Coins!.Include(x => x.Member).Select(x => new
        {
            x.Code,
            x.Value,
            x.Member!.FullName,
            IsAlreadyScavenged = x.MemberId != null
        }));
    }
    
    [HttpGet]
    [Route("data/reset")]
    public ActionResult ResetData()
    {
        appDbContext.ScanSessions.ExecuteDelete();
        appDbContext.ScanCoins.ExecuteDelete();
        
        foreach (var coin in appDbContext.Coins)
        {
            coin.MemberId = null;
            coin.LockUntil = null;
        }

        appDbContext.SaveChanges();
        
        return Ok();
    }
    
    [HttpGet]
    [Route("app-test-mode")]
    public ActionResult IsAppTestMode()
    {
        return Ok(scoutsAppEnvironment.ScoutsAppMode == ScoutsAppMode.AcceptanceTest);
    }
}