// dotcover disable

using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Middleware;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
[AdminAuth]
public class AdminController(IAppDbContext appDbContext) : ControllerBase
{
    private static readonly object Locker = new();

    [HttpPost]
    [Route("ScoutGroup")]
    public ActionResult CreateScoutGroup([FromBody] CreateScoutGroupViewModel createScoutGroupViewModel)
    {
        var scoutGroup = appDbContext.CreateScoutGroup(createScoutGroupViewModel.Id, createScoutGroupViewModel.Name);

        return Ok($"ScoutGroup {scoutGroup.Name} added");
    }

    [HttpPost]
    [Route("Member")]
    public object CreateMember([FromBody] CreateMemberViewModel createMemberViewModel)
    {
        lock (Locker)
        {
            return Ok(appDbContext.CreateMember(
                createMemberViewModel.FirstName,
                createMemberViewModel.LastName,
                createMemberViewModel.ScoutGroupId,
                createMemberViewModel.Section, // Todo: Client sends "section" but this is really "sectionId"
                createMemberViewModel.IsDayVisitor));
        }
    }

    [HttpPut]
    [Route("Member/{memberId:int}")]
    public object UpdateMemberName(int memberId, [FromBody] UpdateMemberViewModel updateMemberViewModel)
    {
        var member = appDbContext.UpdateMemberName(memberId, updateMemberViewModel.FirstName, updateMemberViewModel.LastName);
        
        return new
        {
            MemberNumber = member.Number,
            MemberID = member.Id,
        };
    }

    [HttpGet]
    [Route("ClueStatus")]
    public object GetClueStatus(int memberId)
    {
        var member = appDbContext.Members!.Single(x => x.Id == memberId);

        return new
        {
            MemberId = memberId,
            member.Clue1State,
            member.Clue2State,
            member.Clue3State
        };
    }

    [HttpPost]
    [Route("Coins")]
    public ActionResult CreateCoins([FromBody] CreateCoinViewModel createCoinViewModel)
    {
        if (createCoinViewModel.BaseId.HasValue && !string.IsNullOrWhiteSpace(createCoinViewModel.BaseName))
        {
            return BadRequest("Both BaseId and BaseName were provided. Provide one.");
        }

        if (!createCoinViewModel.BaseId.HasValue && string.IsNullOrWhiteSpace(createCoinViewModel.BaseName))
        {
            return BadRequest("Either BaseId or BaseName is required.");
        }

        if (createCoinViewModel.Points == 0)
        {
            return BadRequest("Points must be provided.");
        }

        var baseId = string.IsNullOrWhiteSpace(createCoinViewModel.BaseName)
            ? createCoinViewModel.BaseId!.Value
            : appDbContext.ActivityBases!.Single(x => x.Name == createCoinViewModel.BaseName).Id!;

        var coins = appDbContext.CreateCoins(baseId!, createCoinViewModel.Points, createCoinViewModel.Count);

        return Ok(coins);
    }
}