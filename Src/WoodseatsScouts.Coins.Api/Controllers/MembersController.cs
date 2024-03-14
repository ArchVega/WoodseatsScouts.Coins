using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MembersController : ControllerBase
{
    private readonly IAppDbContext appDbContext;
    private readonly AppConfig appConfig;
    private readonly IWebHostEnvironment webHostEnvironment;

    public MembersController(IAppDbContext appDbContext, AppConfig appConfig, IWebHostEnvironment webHostEnvironment)
    {
        this.appDbContext = appDbContext;
        this.appConfig = appConfig;
        this.webHostEnvironment = webHostEnvironment;
    }
    
    [HttpGet]
    public OkObjectResult GetMembersWithPoints()
    {
        return Ok(appDbContext.Members!
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScavengedCoins)
            .Include(x => x.Troop)
            .Include(x => x.Section)
            .Select(x => new
            {
                x.Id,
                MemberCode = x.Code,
                x.HasImage,
                MemberNumber = x.Number,
                x.FirstName,
                x.LastName,
                TroopName = x.Troop.Name,
                Section = x.SectionId,
                SectionName = x.Section.Name,
                TotalPoints = x.ScavengeResults.SelectMany(y => y.ScavengedCoins.Select(z => z.PointValue)).Sum()
            })
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName));
    }

    [HttpGet]
    [Route("{code}")]
    public IActionResult GetMemberInfoFromCode(string code)
    {
        try
        {
            var translationResult = CodeTranslator.TranslateMemberCode(code);
            var member = appDbContext.Members!
                .Single(x => x.Number == translationResult.MemberNumber
                             && x.TroopId == translationResult.TroopNumber
                             && x.SectionId == translationResult.Section);

            // The QRScanner for coins becomes active after 500ms after a member has logged in. Slight delay to allow the admin to shift focus away.
            Thread.Sleep(2000);

            return Ok(new
            {
                member.FirstName,
                member.LastName,
                MemberPhotoPath = $"/member-images/{member.Id}.jpg",
                MemberTroopNumber = member.TroopId,
                MemberSection = member.SectionId,
                MemberId = member.Id,
                MemberCode = code,
                MemberNumber = member.Number
            });
        }
        catch (CodeTranslationException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"A member with the code '{code}' was not found.");
        }
    }

    [HttpPut]
    [Route("{id:int}/Coins")]
    public ActionResult AddPointsToMember(int id, [FromBody] PointsForMemberViewModel viewModel)
    {
        var member = appDbContext.Members!.Single(x => x.Id == id);

        var tallyHistoryItem = new ScavengeResult()
        {
            MemberId = member.Id,
            CompletedAt = DateTime.Now
        };

        appDbContext.ScavengeResults!.Add(tallyHistoryItem);

        appDbContext.SaveChanges();

        foreach (var coinCode in viewModel.CoinCodes)
        {
            try
            {
                var result = CodeTranslator.TranslateCoinPointCode(coinCode);

                appDbContext.ScavengedCoins?.Add(new ScavengedCoin
                {
                    ScavengeResultId = tallyHistoryItem.Id,
                    BaseNumber = result.BaseNumber,
                    PointValue = result.PointValue,
                    Code = coinCode
                });
            }
            catch (CodeTranslationException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException)
            {
                return NotFound($"A coin with the code '{coinCode}' was not found.");
            }
        }

        appDbContext.SaveChanges();

        return CreatedAtAction(nameof(AddPointsToMember), null, null);
    }

    [HttpPost]
    [Route("{id:int}/Photo")]
    public ActionResult SaveMemberPhoto(int id, [FromBody] SaveMemberPhotoViewModel saveMemberPhotoViewModel)
    {
        var convert = saveMemberPhotoViewModel.Photo.Replace("data:image/jpeg;base64,", string.Empty);
        var rootPath =
            appConfig.ContentRootDirectory
            ?? Path.Join(webHostEnvironment.ContentRootPath, "..", "woodseatsscouts.coins.web", "public", "member-images");
        var photoFileName = $"{id}.jpg";
        var photoFullPath = Path.Join(rootPath, photoFileName);
        System.IO.File.WriteAllBytes(photoFullPath, Convert.FromBase64String(convert));
        appDbContext.Members!.Single(x => x.Id == id).HasImage = true;
        appDbContext.SaveChanges();

        return Ok();
    }
}