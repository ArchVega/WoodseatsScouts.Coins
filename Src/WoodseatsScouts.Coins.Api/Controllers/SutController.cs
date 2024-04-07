// dotcover disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SutController(
    IAppDbContext appDbContext,
    SystemDateTimeProvider systemDateTimeProvider,
    IOptions<LeaderboardSettings> leaderboardSettingsOptions) : ControllerBase
{
    private readonly LeaderboardSettings leaderboardSettings = leaderboardSettingsOptions.Value;

    [HttpGet]
    [Route("Members")]
    public List<Member> GetMembers()
    {
        return appDbContext.Members!.ToList();
    }

    [HttpPut]
    [Route("Members/HasImage/True")]
    public IActionResult SetAllMemberHasImagePropertyToTrue()
    {
        var members = appDbContext.Members!.ToList();
        foreach (var member in members)
        {
            member.HasImage = true;
        }

        appDbContext.SaveChanges();

        return Ok("Updated all members HasImage property to true");
    }
    
    [HttpPut]
    [Route("Leaderboard/Deadline/{minutesToAdd:int}")]
    public IActionResult SetReportDeadline(int minutesToAdd)
    {
        leaderboardSettings.ScavengerHuntDeadline = DateTime.Now.AddMinutes(minutesToAdd);

        return Ok($"Report deadline datetime set to '{leaderboardSettings.ScavengerHuntDeadline}'");
    }
    
    [HttpPut]
    [Route("SystemDateTime/{minutesToAdd:int?}")]
    public IActionResult SetSystemDateTime(int? minutesToAdd)
    {
        if (minutesToAdd.HasValue)
        {
            systemDateTimeProvider.SetDateTimeToSetTime(DateTime.Now.AddMinutes(minutesToAdd.Value));
        }
        else
        {
            systemDateTimeProvider.SetDateTimeToSystemClock();
        }

        return Ok($"System datetime set to '{systemDateTimeProvider.Now}'");
    }
    
    [HttpGet]
    [Route("Coins")]
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
    [Route("ResetData")]
    public ActionResult ResetData()
    {
        appDbContext.ScavengeResults.ExecuteDelete();
        appDbContext.ScavengedCoins.ExecuteDelete();
        foreach (var coin in appDbContext.Coins)
        {
            coin.MemberId = null;
            coin.LockUntil = null;
        }

        appDbContext.SaveChanges();
        
        return Ok();
    }
}