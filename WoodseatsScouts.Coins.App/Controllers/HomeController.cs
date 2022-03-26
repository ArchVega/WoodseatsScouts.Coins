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

        var scout = context.Scouts!
            .Single(x => x.ScoutNumber == translationResult.ScoutNumber);

        return new
        {
            ScoutName = scout.Name,
            ScoutPhotoPath = $"/scout-images/{scout.ScoutNumber}.jpg",
            ScoutTroopNumber = scout.TroopNumber, 
            ScoutSection = scout.Section
        };
    }

    [HttpGet]
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
    public void AddPointsToScout(PointsForScoutViewModel viewModel)
    {
        var scoutResult = CodeTranslator.TranslateScoutCode(viewModel.ScoutCode);
        
        var scout = context.Scouts.Single(x => x.ScoutNumber == scoutResult.ScoutNumber);
        
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
}