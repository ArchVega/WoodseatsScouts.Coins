using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

#if (DEBUG || ACCEPTANCETEST)
    [HttpGet]
    [Route("Get")]
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