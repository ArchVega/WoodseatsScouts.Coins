using Microsoft.AspNetCore.Mvc;
using WoodseatsScouts.Coins.App.AppLogic.Translators;
using WoodseatsScouts.Coins.App.Data;
using WoodseatsScouts.Coins.App.Models;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext context;

    public HomeController(AppDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public object GetScoutInfoFromCode(string code)
    {
        var translationResult = CodeTranslator.TranslateScoutCode(code);

        //
        var scout = context.Scouts!
            .Single(x => x.ScoutNumber == translationResult.ScoutNumber
                      && x.TroopNumber == translationResult.TroopNumber
                      && x.Section == translationResult.Section);

        //

        //var scout = context.Scouts!
        //    .Single(x => x.ScoutNumber == translationResult.ScoutNumber);

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
    public object GetPointValueFromCode(string code)
    {
        var result = CodeTranslator.TranslateCoinPointCode(code);
        if (result.PointValue > 50)
        {
            throw new ArgumentException("Points value is greater than 50");
        }
        return new
        {
            result.PointValue,
            result.BaseNumber,
            Code = code
        };
    }

    [HttpPost]
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
        //else
        //{
        //    NextSectionMemberNumber =  1;
        //}

       
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
    public object GetClueStatus(int ScoutID)
    {
       var scout = context.Scouts!
            .Single(x => x.Id == ScoutID);

        return new
        {
            ScoutID = ScoutID,
            Clue1State = scout.Clue1State,
            Clue2State = scout.Clue2State,
            Clue3State = scout.Clue3State
        };
    }
}