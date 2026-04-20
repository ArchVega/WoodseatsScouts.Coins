using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Queries;
using WoodseatsScouts.Coins.Api.Models.View;
using WoodseatsScouts.Coins.Api.Models.View.Members;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Route("api/members")]
public class MemberController(
    IMemberService memberService,
    IAppDbContext appDbContext,
    IImagePersister imagePersister,
    IOptions<AppSettings> appSettingsOptions,
    IOptions<LeaderboardSettings> leaderboardSettingsOptions) : ControllerBase
{
    [HttpGet]
    [Route("{code}")]
    public IActionResult GetMemberByCode(string code, [FromQuery] MemberQuery? memberQuery)
    {
        memberQuery ??= new MemberQuery
        {
            MemberQueryView = MemberQueryView.Basic
        };
        
        MemberCodeTranslationResult translationResult;
        try
        {
            translationResult = CodeTranslator.TranslateMemberCode(code);
        }
        catch (CodeTranslationException e)
        {
            return BadRequest(e.Message);
        }

        var memberId = memberService.GetMemberId(translationResult.MemberNumber, translationResult.ScoutGroupNumber, translationResult.Section);

        switch (memberQuery.MemberQueryView)
        {
            case MemberQueryView.Login:
                /*  The QRScanner for coins becomes active after 500ms after a member has logged in.
                    Slight delay to allow the admin to shift focus away. */
                Thread.Sleep(appSettingsOptions.Value.LoginPauseDurationSeconds * 1000);
                return Ok(memberService.GetMemberDto(memberId));
            case MemberQueryView.Basic:
                return Ok(memberService.GetMemberDto(memberId));
            case MemberQueryView.PointsSummary:
                return Ok(memberService.MemberPointsSummaryDto(memberId));
            case MemberQueryView.Complete:
                return Ok(memberService.MemberCompleteSummaryDto(memberId));
            default:
                throw new ArgumentOutOfRangeException(nameof(memberQuery));
        }
    }

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
    [Route("{code}/WithPoints")]
    public MembersWithPointsViewModel GetMemberWithPoints(string code)
    {
        var member = appDbContext.Members!
            .Include(x => x.Section)
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScavengedCoins)
            .Single(x => x.Code == code);

        var viewModel = new MembersWithPointsViewModel(member);

        if (member.ScavengeResults.Any())
        {
            viewModel.LatestCompletedAtTime = member.ScavengeResults.MaxBy(x => x.CompletedAt).CompletedAt;
        }

        return viewModel;
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
        var hasMemberImage = memberService.HasMemberImage(id);

        if (!hasMemberImage)
        {
            return File(imagePersister.PlaceholderImageStream(), "image/png", enableRangeProcessing: true);
        }
        
        var stream = imagePersister.RetrieveImageBytes(id);
        return File(stream, "image/jpeg", enableRangeProcessing: true);
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
        var latest6Scavengers = appDbContext.GetLatestScans(leaderboardSettingsOptions.Value.NumberOfLatestScansToDisplay);

        return Ok(latest6Scavengers);
    }
}