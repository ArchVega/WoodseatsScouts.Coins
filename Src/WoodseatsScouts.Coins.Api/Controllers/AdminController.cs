using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private static readonly object Locker = new object();

    private readonly AppDbContext appDbContext;

    public AdminController(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    [HttpPost]
    [Route("Troop")]
    public ActionResult CreateTroop([FromBody] CreateTroopViewModel createTroopViewModel)
    {
        using var transaction = appDbContext.Database.BeginTransaction();
        appDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Troops ON");

        appDbContext.Troops?.Add(new Troop
        {
            Id = createTroopViewModel.Id,
            Name = createTroopViewModel.Name
        });

        appDbContext.SaveChanges();

        appDbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Troops OFF");
        transaction.Commit();

        return Ok($"Troop {createTroopViewModel.Name} added");
    }

    [HttpPost]
    [Route("Member")]
    public object CreateMember([FromBody] CreateMemberViewModel createMemberViewModel)
    {
        lock (Locker)
        {
            var memberNumber =
                appDbContext.GenerateNextMemberCode(createMemberViewModel.TroopId, createMemberViewModel.Section);

            appDbContext.Members?.Add(new Member
            {
                Number = memberNumber,
                FirstName = createMemberViewModel.FirstName,
                LastName = createMemberViewModel.LastName,
                TroopId = createMemberViewModel.TroopId,
                SectionId = createMemberViewModel.Section,
                IsDayVisitor = createMemberViewModel.IsDayVisitor
            });

            appDbContext.SaveChanges();

            var member = appDbContext.Members!
                .Single(x => x.Number == memberNumber
                             && x.TroopId == createMemberViewModel.TroopId
                             && x.SectionId == createMemberViewModel.Section);

            return new
            {
                MemberNumber = member.Number,
                MemberID = member.Id,
            };
        }
    }

    [HttpGet]
    [Route("Member/{memberId:int}")]
    public object UpdateMemberName(int memberId)
    {
        var member = appDbContext.Members!.SingleOrDefault(x => x.Id == memberId);

        if (member == null)
        {
            return NotFound(memberId);
        }

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
    [Route("GetClueStatus")]
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