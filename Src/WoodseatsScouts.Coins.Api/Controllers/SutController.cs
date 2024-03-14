using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Controllers;

#if ACCEPTANCETEST
[ApiController]
[Route("[controller]")]
public class SutController : ControllerBase
{
    private readonly IAppDbContext appDbContext;
    private readonly AppConfig appConfig;

    public SutController(IAppDbContext appDbContext, AppConfig appConfig)
    {
        this.appDbContext = appDbContext;
        this.appConfig = appConfig;
    }

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
    [Route("Leaderboard/Deadline/{daysToAdd:int}")]
    public IActionResult SetReportDeadline(int daysToAdd)
    {
        appConfig.ReportDeadline = DateTime.Now.AddDays(daysToAdd);

        return Ok($"Report deadline datetime set to '{appConfig.ReportDeadline}'");
    }
    
#if (DEBUG || ACCEPTANCETEST)
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
#endif
}
#endif