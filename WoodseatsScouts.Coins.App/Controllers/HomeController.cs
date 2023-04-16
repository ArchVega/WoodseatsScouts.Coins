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
    public void AddPointsToScout(PointsForScoutViewModel viewModel)
    {
        var translationResult = CodeTranslator.TranslateScoutCode(viewModel.ScoutCode);

        var scout = context.Scouts!
            .Single(x => x.ScoutNumber == translationResult.ScoutNumber
                      && x.TroopNumber == translationResult.TroopNumber
                      && x.Section == translationResult.Section);

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
    }

    [HttpPost]
    [Route("CreateMember")]
    public object CreateMember(String Name, int TroopNumber, String Section)
    {
        var SectionMembers = context.Scouts!
            .Where(x => x.TroopNumber == TroopNumber
                      && x.Section == Section);

        var NextSectionMemberNumber = 1;

        if (SectionMembers.FirstOrDefault() != null)
        {
            NextSectionMemberNumber = SectionMembers.Max(m => m.ScoutNumber) + 1;
        }
       
        context.Scouts?.Add(new Scout
        {
            ScoutNumber = NextSectionMemberNumber,
            Name = Name,
            TroopNumber = TroopNumber,
            Section = Section
        });

        context.SaveChanges();

        var scout = context.Scouts!
            .Single(x => x.ScoutNumber == NextSectionMemberNumber
                      && x.TroopNumber == TroopNumber
                      && x.Section == Section);

        return new
        {
            MemberNumber = scout.ScoutNumber,
            MemberID = scout.Id,
        };

    }

    [HttpPut]
    [Route("UpdateMemberName")]
    public object UpdateMemberName(int ScoutID, String Name)
    {
        var entityOrNull = context.Scouts!.SingleOrDefault(x => x.Id == ScoutID);

        if(entityOrNull != null) {
            entityOrNull.Name = Name;
            // other updates here it there are any
            context.SaveChanges();
        } else
        {
            throw new ArgumentException("ScoutID not found");
        }

        var scout = context.Scouts!
            .Single(x => x.Id == ScoutID);

        return new
        {
            MemberNumber = scout.ScoutNumber,
            MemberID = scout.Id,
        };

    }

    [HttpGet]
    [Route("GetClueStatus")]
    public object GetClueStatus(int ScoutID)
    {
       var scout = context.Scouts!
            .Single(x => x.Id == ScoutID);

        return new
        {
            ScoutID,
            scout.Clue1State,
            scout.Clue2State,
            scout.Clue3State
        };
    }
    
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}