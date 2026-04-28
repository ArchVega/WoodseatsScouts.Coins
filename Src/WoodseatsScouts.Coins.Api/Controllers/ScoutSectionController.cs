using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts;
using WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Groups;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scout Sections")]
[Route("api/scouts/sections")]
public class ScoutSectionController( IAppDbContext appDbContext) : ControllerBase
{   
    [HttpGet]
    [Route("")]
    public ActionResult ScoutSections()
    {
        return Ok(appDbContext.ScoutSections!.Select(x => new ScoutSectionDto(x)));
    }
    
    [HttpPost]
    [Route("")]
    public ActionResult CreateScoutSection([FromBody] CreateScoutSectionRequest createScoutSectionRequest)
    {
        var scoutSection = appDbContext.CreateScoutSection(createScoutSectionRequest.Code, createScoutSectionRequest.Name);

        return Created("", scoutSection);
    }
}