using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.View;
using WoodseatsScouts.Coins.Api.Models.View.Members;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MembersController(
    IAppDbContext appDbContext,
    IOptions<LeaderboardSettings> leaderboardSettingsOptions,
    IImagePersister imagePersister) : ControllerBase
{
    [HttpGet]
    public ActionResult GetMembersWithPoints()
    {
        return Ok(appDbContext.Members!
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScavengedCoins)
            .Include(x => x.ScoutGroup)
            .Include(x => x.Section)
            .ToList()
            .Select(x => new MembersWithPointsViewModel(x))
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToList());
    }

    [HttpGet]
    [Route("{code}")]
    public IActionResult GetMemberInfoFromCode(string code)
    {
        MemberCodeTranslationResult translationResult;
        try
        {
            translationResult = CodeTranslator.TranslateMemberCode(code);
        }
        catch (CodeTranslationException e)
        {
            return BadRequest(e.Message);
        }

        var member = appDbContext.Members!
            .Include(x => x.ScoutGroup)
            .Include(x => x.Section)
            .Single(x => x.Number == translationResult.MemberNumber
                         && x.ScoutGroupId == translationResult.ScoutGroupNumber
                         && x.SectionId == translationResult.Section);

        /* The QRScanner for coins becomes active after 500ms after a member has logged in.
           Slight delay to allow the admin to shift focus away.*/
        // Todo: We don't need to wait between member code QR calls unless we're running in release. Either turn off or reduce.
        Thread.Sleep(2000);

        var memberViewModel = new MemberViewModel(member);
        return Ok(memberViewModel);
    }

    [HttpPut]
    [Route("{id:int}/Coins")]
    public ActionResult AddPointsToMember(int id, [FromBody] PointsForMemberViewModel viewModel)
    {
        // Todo: wrap in a transaction
        var member = appDbContext.Members!.Single(x => x.Id == id);

        var tallyHistoryItem = appDbContext.CreateScavengeResult(member);

        appDbContext.CreateScavengedCoins(tallyHistoryItem, viewModel.CoinCodes);

        var alreadyScavengedCoins = appDbContext.RecordMemberAgainstUnscavengedCoins(member, viewModel.CoinCodes);

        var responseViewModel = new AddPointsToMemberViewModel(alreadyScavengedCoins);

        return CreatedAtAction(nameof(AddPointsToMember), null, responseViewModel);
    }

    [HttpGet]
    [Route("{id:int}/Photo")]
    public IActionResult Get(int id)
    {
        var bytes = imagePersister.RetrieveImageBytes(id);

        return File(bytes, "image/jpeg", $"{id}.jpg");
    }

    [HttpPut]
    [Route("{id:int}/Photo")]
    public ActionResult SaveMemberPhoto(int id, [FromBody] SaveMemberPhotoViewModel saveMemberPhotoViewModel)
    {
        imagePersister.Persist(id.ToString(), saveMemberPhotoViewModel.Photo);
        appDbContext.Members!.Single(x => x.Id == id).HasImage = true;
        appDbContext.SaveChanges();

        return Ok();
    }

    [HttpPost]
    [Route("{id:int}/Name")]
    public ActionResult Name(int id, [FromBody] UpdateMemberNameViewModel updateMemberNameViewModel)
    {
        var member = appDbContext.Members!.Single(x => x.Id == id);
        member.FirstName = updateMemberNameViewModel.FirstName;
        member.LastName = updateMemberNameViewModel.LastName;
        appDbContext.SaveChanges();

        return Ok();
    }
    
    [HttpGet]
    [Route("RefreshSecondsForLatestScans")]
    public ActionResult RefreshSecondsForLatestScans()
    {
        return Ok(leaderboardSettingsOptions.Value.Last6ScavengersPageRefreshSeconds);
    }
    
    [HttpGet]
    [Route("LatestScans")]
    public ActionResult LatestScans()
    {
        var latest6Scavengers = appDbContext.GetLastSixScavengers();
        
        return Ok(latest6Scavengers);
    }
}