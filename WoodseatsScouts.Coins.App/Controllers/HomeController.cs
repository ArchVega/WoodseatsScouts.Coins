using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.App.AppLogic.Translators;
using WoodseatsScouts.Coins.App.Data;
using WoodseatsScouts.Coins.App.Models;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly AppDbContext context;

    private readonly ILogger<HomeController> logger;

    public HomeController(AppDbContext context, ILogger<HomeController> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    [HttpGet]
    [Route("GetScoutInfoFromCode")]
    public object GetScoutInfoFromCode(string code)
    {
        var translationResult = CodeTranslator.TranslateScoutCode(code);

        var scout = context.Scouts!
            .Single(x => x.ScoutNumber == translationResult.ScoutNumber
                      && x.TroopNumber == translationResult.TroopNumber
                      && x.Section == translationResult.Section);

        return new
        {
            ScoutName = scout.Name,
            ScoutPhotoPath = $"/scout-images/{scout.Id}.jpg",
            ScoutTroopNumber = scout.TroopNumber, 
            ScoutSection = scout.Section,
            ScoutID = scout.Id,
            ScoutNumber = scout.ScoutNumber
        };
    }

    [HttpGet]
    [Route("GetPointValueFromCode")]
    public object GetPointValueFromCode(string code)
    {
        var result = CodeTranslator.TranslateCoinPointCode(code);
        return new
        {
            result.PointValue,
            result.BaseNumber,
            Code = code
        };
    }

    [HttpPost]
    [Route("AddPointsToScout")]
    public ActionResult AddPointsToScout([FromBody] PointsForScoutViewModel viewModel)
    {
        var scout = context.Scouts!.Single(x => x.Id == viewModel.ScoutId);

        foreach (var coinCode in viewModel.CoinCodes)
        {
            var result = CodeTranslator.TranslateCoinPointCode(coinCode);

            context.ScoutPoints?.Add(new ScoutPoint
            {
                ScoutId = scout.Id,
                BaseNumber = result.BaseNumber,
                PointValue = result.PointValue,
                ScannedCode = coinCode
            });
        }

        context.SaveChanges();

        return CreatedAtAction(nameof(AddPointsToScout), null, null);
    }

    [HttpPost]
    [Route("CreateMember")]
    public object CreateMember(string name, int troopNumber, string section)
    {
        var sectionMembers = context.Scouts!
            .Where(x => x.TroopNumber == troopNumber
                      && x.Section == section);

        var nextSectionMemberNumber = 1;

        if (sectionMembers.FirstOrDefault() != null)
        {
            nextSectionMemberNumber = sectionMembers.Max(m => m.ScoutNumber) + 1;
        }
       
        context.Scouts?.Add(new Scout
        {
            ScoutNumber = nextSectionMemberNumber,
            Name = name,
            TroopNumber = troopNumber,
            Section = section
        });

        context.SaveChanges();

        var scout = context.Scouts!
            .Single(x => x.ScoutNumber == nextSectionMemberNumber
                      && x.TroopNumber == troopNumber
                      && x.Section == section);

        return new
        {
            MemberNumber = scout.ScoutNumber,
            MemberID = scout.Id,
        };

    }

    [HttpPut]
    [Route("UpdateMemberName")]
    public object UpdateMemberName(int scoutId, string name)
    {
        var entityOrNull = context.Scouts!.SingleOrDefault(x => x.Id == scoutId);

        if(entityOrNull != null) {
            entityOrNull.Name = name;
            // other updates here it there are any
            context.SaveChanges();
        } else
        {
            throw new ArgumentException("ScoutID not found");
        }

        var scout = context.Scouts!
            .Single(x => x.Id == scoutId);

        return new
        {
            MemberNumber = scout.ScoutNumber,
            MemberID = scout.Id,
        };

    }

    [HttpGet]
    [Route("GetClueStatus")]
    public object GetClueStatus(int scoutId)
    {
       var scout = context.Scouts!
            .Single(x => x.Id == scoutId);

        return new
        {
            ScoutID = scoutId,
            scout.Clue1State,
            scout.Clue2State,
            scout.Clue3State
        };
    }
}