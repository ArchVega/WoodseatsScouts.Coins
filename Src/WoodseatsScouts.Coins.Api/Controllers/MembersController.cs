using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MembersController(
    IAppDbContext appDbContext,
    IOptions<AppSettings> appSettingsOptions,
    IImagePersister imagePersister) : ControllerBase
{
    private static readonly object Locker = new();

    [HttpGet]
    public ActionResult GetMembersWithPoints()
    {
        return Ok(appDbContext.Members!
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScavengedCoins)
            .Include(x => x.Troop)
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
            .Single(x => x.Number == translationResult.MemberNumber
                         && x.TroopId == translationResult.TroopNumber
                         && x.SectionId == translationResult.Section);

        /* The QRScanner for coins becomes active after 500ms after a member has logged in.
           Slight delay to allow the admin to shift focus away.*/
        // Todo: We don't need to wait between member code QR calls unless we're running in release. Either turn off or reduce.
        Thread.Sleep(2000);

        var wtf = new MemberViewModel(member);
        return Ok(wtf);
    }

    [HttpGet]
    [Route("{code}/Vote")]
    public IActionResult GetMemberInfoFromCodeForVoting(string code)
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
            .Single(x => x.Number == translationResult.MemberNumber
                         && x.TroopId == translationResult.TroopNumber
                         && x.SectionId == translationResult.Section);

        var alreadyVoted = appDbContext.MemberCountryVotes!.Any(x => x.MemberId == member.Id);

        if (alreadyVoted)
        {
            return BadRequest($"{member.FirstName} has already casted their vote!");
        }

        /* The other method (for logging in to scan coins) has a slight delay. We might not need this delay here, but keeping it here for
         consistency for the UX.*/
        // Todo: We don't need to wait between member code QR calls unless we're running in release. Either turn off or reduce.
        Thread.Sleep(2000);

        return Ok(new MemberViewModel(member));
    }

    [HttpPut]
    [Route("{id:int}/Coins")]
    public ActionResult AddPointsToMember(int id, [FromBody] PointsForMemberViewModel viewModel)
    {
        lock (Locker)
        {
            // Todo: wrap in a transaction
            var member = appDbContext.Members!.Single(x => x.Id == id);

            var existingScavengeResultItems = appDbContext.ScavengeResults!.Where(x => x.MemberId == member.Id).ToList();
            if (existingScavengeResultItems.Count > 0)
            {
                var latestScavengedResultItem = existingScavengeResultItems.OrderByDescending(x => x.ScavengedCoins).First();
                var lockedUntilTime = latestScavengedResultItem.CompletedAt + TimeSpan.FromSeconds(appSettingsOptions.Value.SecondsBetweenScavengerHauls);
                if (DateTime.Now < lockedUntilTime)
                {
                    Console.WriteLine("Multiple executions were attempted, aborting");
                    throw new ApplicationException($"Oops, your coins have already been stashed!");
                }
            }

            var tallyHistoryItem = appDbContext.CreateScavengeResult(member);
            appDbContext.CreateScavengedCoins(tallyHistoryItem, viewModel.CoinCodes);
            var alreadyScavengedCoins = appDbContext.RecordMemberAgainstUnscavengedCoins(member, viewModel.CoinCodes);
            var responseViewModel = new AddPointsToMemberViewModel(alreadyScavengedCoins);
            
            Console.WriteLine($"Scavenged haul for member {member.FullName} recorded");
            return CreatedAtAction(nameof(AddPointsToMember), null, responseViewModel);
        }
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
}