using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;
using WoodseatsScouts.Coins.Api.Models.Queries;
using WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Controllers;

[ApiController]
[Tags("Scout Members")]
[Route("api/scouts/members")]
public class ScoutMemberController(
    IScoutMemberService scoutMemberService,
    IAppDbContext appDbContext,
    IImagePersister imagePersister,
    IOptions<AppSettings> appSettingsOptions) : ControllerBase
{
    private static readonly Lock Locker = new();
    
    /// <summary>
    /// Gets all scout members.
    /// </summary>
    [HttpGet]
    [Route("")]
    public ActionResult GetAllScoutMembers()
    {
        return Ok(scoutMemberService.ScoutGetMemberWithPointsSummaryDtos());
    }

    /// <summary>
    /// Creates a new scout member.
    /// </summary>
    [HttpPost]
    [Route("")]
    public IActionResult CreateScoutMember([FromBody] CreateMemberRequest createMemberRequest)
    {
        lock (Locker)
        {
            return Ok(appDbContext.CreateMember(
                createMemberRequest.FirstName,
                createMemberRequest.LastName,
                createMemberRequest.ScoutGroupId,
                createMemberRequest.SectionCode,
                createMemberRequest.IsDayVisitor));
        }
    }

    /// <summary>
    /// Assign coins to a scout member. 
    /// </summary>
    [HttpPut]
    [Route("{scoutMemberId:int}/coins")]
    public ActionResult AssignCoinsToScoutMember(int scoutMemberId, [FromBody] AssignCoinsToScoutMembersRequest request)
    {
        // Todo: wrap in a transaction
        var member = appDbContext.ScoutMembers!.Single(x => x.Id == scoutMemberId);

        var tallyHistoryItem = appDbContext.CreateScavengeResult(member);

        appDbContext.CreateScavengedCoins(tallyHistoryItem, request.CoinCodes);

        var alreadyScavengedCoins = appDbContext.RecordMemberAgainstUnscavengedCoins(member, request.CoinCodes);

        var addPointsToMemberDto = new AddPointsToMemberDto(alreadyScavengedCoins);

        return CreatedAtAction(nameof(AssignCoinsToScoutMember), null, addPointsToMemberDto);
    }

    /// <summary>
    /// Gets a scout member's photo. 
    /// </summary>
    [HttpGet]
    [Route("{scoutMemberId:int}/photo")]
    public IActionResult GetScoutMemberPhoto(int scoutMemberId)
    {
        var stream = imagePersister.RetrieveImageBytes(scoutMemberId);
        return File(stream, "image/jpeg", enableRangeProcessing: true);
    }

    /// <summary>
    /// Updates a scout member's photo.
    /// </summary>
    [HttpPut]
    [Route("{scoutMemberId:int}/photo")]
    public ActionResult SaveScoutMemberPhoto(int scoutMemberId, [FromBody] SaveMemberPhotoRequestModel saveMemberPhotoRequestModel)
    {
        imagePersister.Persist(scoutMemberId.ToString(), saveMemberPhotoRequestModel.Photo);
        appDbContext.ScoutMembers!.Single(x => x.Id == scoutMemberId).HasImage = true;
        appDbContext.SaveChanges();

        return Ok();
    }

    /// <summary>
    /// Get a scout member's details by their member code.
    /// </summary>
    [HttpGet]
    [Route("{scoutMemberCode}")]
    public IActionResult GetScoutMemberByCode(string scoutMemberCode, [FromQuery] Member? memberQuery)
    {
        memberQuery ??= new Member
        {
            View = View.Basic
        };

        MemberCodeTranslationResult translationResult;
        try
        {
            translationResult = CodeTranslator.TranslateMemberCode(scoutMemberCode);
        }
        catch (CodeTranslationException e)
        {
            return BadRequest(e.Message);
        }

        var memberId = scoutMemberService.GetScoutMemberId(translationResult.ScoutMemberNumber, translationResult.ScoutGroupId, translationResult.ScoutSectionCode);

        switch (memberQuery.View)
        {
            case View.Login:
                /*  The QRScanner for coins becomes active after 500ms after a member has logged in. Slight delay to allow the admin to shift focus away. */
                Thread.Sleep(appSettingsOptions.Value.LoginPauseDurationSeconds * 1000);
                return Ok(scoutMemberService.GetMemberDto(memberId));
            case View.Basic:
                return Ok(scoutMemberService.GetMemberDto(memberId));
            case View.PointsSummary:
                return Ok(scoutMemberService.ScoutMemberPointsSummaryDto(memberId));
            case View.Complete:
                return Ok(scoutMemberService.MemberCompleteSummaryDto(memberId));
            default:
                throw new ArgumentOutOfRangeException(nameof(memberQuery));
        }
    }

    /// <summary>
    /// Update a scout member's details.
    /// </summary>
    [HttpPut]
    [Route("{scoutMemberId:int}")]
    public ActionResult UpdateScoutMember(int scoutMemberId, [FromBody] UpdateScoutMemberRequest request)
    {
        var scoutMember = appDbContext.ScoutMembers.Single(x => x.Id == scoutMemberId);

        var maxMemberNumberFromTargetGroup = appDbContext.ScoutMembers
            .Where(x => x.ScoutGroupId == request.ScoutGroupId && x.ScoutSectionCode == request.ScoutSectionCode)
            .ToList();
            
        var newMemberNumber = maxMemberNumberFromTargetGroup.Count == 0 ?  1 : maxMemberNumberFromTargetGroup.Max(x => x.Number) + 1;
        
        scoutMember.FirstName = request.FirstName;
        scoutMember.LastName = request.LastName;
        scoutMember.ScoutGroupId = request.ScoutGroupId;
        scoutMember.ScoutSectionCode = request.ScoutSectionCode;
        scoutMember.Number = newMemberNumber;
        appDbContext.SaveChanges();
        
        var updatedScoutMember = appDbContext
            .ScoutMembers
            .Include(x => x.ScoutGroup)
            .Include(x => x.ScoutSection)
            .Single(x => x.Id == scoutMemberId);

        return Ok(new ScoutMemberDto(updatedScoutMember));
    }

    /// <summary>
    /// Gets a member's clue statuses.
    /// </summary>
    [HttpGet]
    [Route("clues/status/{scoutMemberId:int}")]
    public IActionResult GetScoutMemberClueStatus(int scoutMemberId)
    {
        var member = appDbContext.ScoutMembers.Single(x => x.Id == scoutMemberId);

        return Ok(new
        {
            MemberId = scoutMemberId,
            member.Clue1State,
            member.Clue2State,
            member.Clue3State
        });
    }
    
    /// <summary>
    /// Gets the default placeholder image for scout members who have no photo. 
    /// </summary>
    [HttpGet]
    [Route("photo/placeholder")]
    public IActionResult GetScoutMemberPlaceholderPhoto()
    {
        return File(imagePersister.PlaceholderImageStream(), "image/png", enableRangeProcessing: true);
    }
}