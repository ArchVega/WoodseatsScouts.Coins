using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Api.Controllers;

#if ACCEPTANCETEST
[ApiController]
[Route("[controller]")]
public class SutController : ControllerBase
{
    private readonly IAppDbContext context;
    private readonly AppConfig appConfig;

    public SutController(IAppDbContext context, AppConfig appConfig)
    {
        this.context = context;
        this.appConfig = appConfig;
    }

    [HttpPut]
    [Route("SetAllMemberHasImagePropertyToTrue")]
    public IActionResult SetAllMemberHasImagePropertyToTrue()
    {
        var members = context.Members!.ToList();
        foreach (var member in members)
        {
            member.HasImage = true;
        }

        context.SaveChanges();

        return Ok("Updated all members HasImage property to true");
    }
    
    [HttpPut]
    [Route("SetReportDeadline/{daysToAdd:int}")]
    public IActionResult SetReportDeadline(int daysToAdd)
    {
        appConfig.ReportDeadline = DateTime.Now.AddDays(daysToAdd);

        return Ok($"Report deadline datetime set to '{appConfig.ReportDeadline}'");
    }
}
#endif