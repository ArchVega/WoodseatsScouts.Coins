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
                createMemberViewModel.Section, // Todo: rename in client?
                createMemberViewModel.IsDayVisitor));
        }
    }

    [HttpGet]
    [Route("Member/{memberId:int}")]
    public object UpdateMemberName(int memberId)
    {
        var member = appDbContext.Members!.Single(x => x.Id == memberId);

        // if (member == null)
        // {
        //     return NotFound(memberId);
        // }

        return Ok(member);
    }

    [HttpPut]
    [Route("Member/{memberId:int}")]
    public object UpdateMemberName(int memberId, [FromBody] UpdateMemberViewModel updateMemberViewModel)
    {
        var entityOrNull = appDbContext.Members!.SingleOrDefault(x => x.Id == memberId);

        if (entityOrNull != null)
        {
            entityOrNull.FirstName = updateMemberViewModel.FirstName;
            entityOrNull.LastName = updateMemberViewModel.LastName;
            appDbContext.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Member id not found");
        }

        var member = appDbContext.Members!
            .Single(x => x.Id == memberId);

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
        var member = appDbContext.Members!
            .Single(x => x.Id == memberId);

        return new
        {
            MemberId = memberId,
            member.Clue1State,
            member.Clue2State,
            member.Clue3State
        };
    }
}