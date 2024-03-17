// dotcover disable
using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController(AppDbContext appDbContext) : ControllerBase
{
    private static readonly object Locker = new();

    [HttpPost]
    [Route("Troop")]
    public ActionResult CreateTroop([FromBody] CreateTroopViewModel createTroopViewModel)
    {
        var troop = appDbContext.CreateTroop(createTroopViewModel.Id, createTroopViewModel.Name);

        return Ok($"Troop {troop.Name} added");
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
                createMemberViewModel.TroopId,
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
}