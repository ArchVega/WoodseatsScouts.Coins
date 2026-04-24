using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;
using WoodseatsScouts.Coins.Api.Models.Queries;
using WoodseatsScouts.Coins.Api.Models.View;
using WoodseatsScouts.Coins.Api.Models.View.Members;

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
    public ActionResult CreateScoutGroup([FromBody] CreateScoutGroupViewModel createScoutGroupViewModel)
    {
        var scoutGroup = appDbContext.CreateScoutGroup(createScoutGroupViewModel.Id, createScoutGroupViewModel.Name);

        return Ok($"ScoutGroup {scoutGroup.Name} added");
    }
}