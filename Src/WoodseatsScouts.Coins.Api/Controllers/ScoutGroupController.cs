using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Api.Models.Dtos.ScoutGroups;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scout Groups")]
[Route("api/scouts/groups")]
public class ScoutGroupController(IAppDbContext appDbContext) : ControllerBase
{   
    [HttpGet]
    [Route("")]
    public ActionResult ScoutGroups()
    {
        return Ok(appDbContext.ScoutGroups!.Select(x => new ScoutGroupDto(x)));
    }
    
    [HttpPost]
    [Route("")]
    public ActionResult CreateScoutGroup([FromBody] CreateScoutGroupRequestModel createScoutGroupRequestModel)
    {
        var scoutGroup = appDbContext.CreateScoutGroup(createScoutGroupRequestModel.Id, createScoutGroupRequestModel.Name);

        return Ok($"ScoutGroup {scoutGroup.Name} added");
    }
}