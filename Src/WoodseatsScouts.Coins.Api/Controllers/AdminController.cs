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
    private readonly AppDbContext context;

    public AdminController(AppDbContext context)
    {
        this.context = context;
    }

    [HttpPost]
    [Route("CreateTroop")]
    public ActionResult CreateTroop([FromBody] CreateTroopViewModel createTroopViewModel)
    {
        using var transaction = context.Database.BeginTransaction();
        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Troops ON");
        
        context.Troops?.Add(new Troop
        {
            Id = createTroopViewModel.Id,
            Name = createTroopViewModel.Name
        });

        context.SaveChanges();
        
        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Troops OFF");
        transaction.Commit();
        
        return Ok($"Troop {createTroopViewModel.Name} added");
    }
}