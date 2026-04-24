using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Queries;
using WoodseatsScouts.Coins.Api.Models.View;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scout Members")]
[Route("api/scouts/members")]
public class ScoutMemberController(
    IMemberService memberService,
    IAppDbContext appDbContext,
    IImagePersister imagePersister,
    IOptions<AppSettings> appSettingsOptions) : ControllerBase
{
    private static readonly object Locker = new();
    
    [HttpGet]
    [Route("{code}")]
    public IActionResult GetMemberByCode(string code, [FromQuery] Member? memberQuery)
    {
        memberQuery ??= new Member
        {
            View = View.Basic
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

        switch (memberQuery.View)
        {
            case View.Login:
                /*  The QRScanner for coins becomes active after 500ms after a member has logged in.
                    Slight delay to allow the admin to shift focus away. */
                Thread.Sleep(appSettingsOptions.Value.LoginPauseDurationSeconds * 1000);
                return Ok(memberService.GetMemberDto(memberId));
            case View.Basic:
                return Ok(memberService.GetMemberDto(memberId));
            case View.PointsSummary:
                return Ok(memberService.MemberPointsSummaryDto(memberId));
            case View.Complete:
                return Ok(memberService.MemberCompleteSummaryDto(memberId));
            default:
                throw new ArgumentOutOfRangeException(nameof(memberQuery));
        }
    }
    
    [HttpPost]
    [Route("")]
    public object CreateMember([FromBody] CreateMemberViewModel createMemberViewModel)
    {
        lock (Locker)
        {
            return Ok(appDbContext.CreateMember(
                createMemberViewModel.FirstName,
                createMemberViewModel.LastName,
                createMemberViewModel.ScoutGroupId,
                createMemberViewModel.Section, // Todo: Client sends "section" but this is really "sectionId"
                createMemberViewModel.IsDayVisitor));
        }
    }

    [HttpGet]
    [Route("")]
    public ActionResult GetAllMembers([FromQuery] Member? memberQuery)
    {
        memberQuery ??= new Member
        {
            View = View.Basic
        };
        
        switch (memberQuery.View)
        {
            case View.PointsSummary:
                return Ok(memberService.GetMemberWithPointsSummaryDtos());
            case View.Login:
            case View.Basic:
            case View.Complete:
            default:
                throw new ArgumentOutOfRangeException(nameof(memberQuery));
        }
    }

    [HttpPut]
    [Route("{id:int}/coins")]
    public ActionResult AddPointsToMember(int id, [FromBody] PointsForMemberViewModel viewModel)
    {
        // Todo: wrap in a transaction
        var member = appDbContext.ScoutMembers!.Single(x => x.Id == id);

        var tallyHistoryItem = appDbContext.CreateScavengeResult(member);

        appDbContext.CreateScavengedCoins(tallyHistoryItem, viewModel.CoinCodes);

        var alreadyScavengedCoins = appDbContext.RecordMemberAgainstUnscavengedCoins(member, viewModel.CoinCodes);

        var responseViewModel = new AddPointsToMemberViewModel(alreadyScavengedCoins);

        return CreatedAtAction(nameof(AddPointsToMember), null, responseViewModel);
    }

    [HttpGet]
    [Route("photo/placeholder")]
    public IActionResult Get()
    {
        return File(imagePersister.PlaceholderImageStream(), "image/png", enableRangeProcessing: true);
    }

    [HttpGet]
    [Route("{id:int}/photo")]
    public IActionResult Get(int id)
    {
        var stream = imagePersister.RetrieveImageBytes(id);
        return File(stream, "image/jpeg", enableRangeProcessing: true);
    }

    [HttpPut]
    [Route("{id:int}/photo")]
    public ActionResult SaveMemberPhoto(int id, [FromBody] SaveMemberPhotoViewModel saveMemberPhotoViewModel)
    {
        imagePersister.Persist(id.ToString(), saveMemberPhotoViewModel.Photo);
        appDbContext.ScoutMembers!.Single(x => x.Id == id).HasImage = true;
        appDbContext.SaveChanges();

        return Ok();
    }
    
    [HttpPost]
    [Route("{id:int}/name")]
    public ActionResult Name(int id, [FromBody] UpdateMemberNameViewModel updateMemberNameViewModel)
    {
        var member = appDbContext.ScoutMembers!.Single(x => x.Id == id);
        member.FirstName = updateMemberNameViewModel.FirstName;
        member.LastName = updateMemberNameViewModel.LastName;
        appDbContext.SaveChanges();

        return Ok();
    }
    
    [HttpPut]
    [Route("{memberId:int}")]
    public object UpdateMemberName(int memberId, [FromBody] UpdateMemberViewModel updateMemberViewModel)
    {
        var member = appDbContext.UpdateMemberName(memberId, updateMemberViewModel.FirstName, updateMemberViewModel.LastName);
        
        return new
        {
            MemberNumber = member.Number,
            MemberID = member.Id,
        };
    }

    [HttpGet]
    [Route("clues/status")]
    public object GetClueStatus(int memberId)
    {
        var member = appDbContext.ScoutMembers!.Single(x => x.Id == memberId);

        return new
        {
            MemberId = memberId,
            member.Clue1State,
            member.Clue2State,
            member.Clue3State
        };
    }
}