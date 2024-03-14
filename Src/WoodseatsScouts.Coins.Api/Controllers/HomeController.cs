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
public class HomeController : ControllerBase
{
    private static readonly object Locker = new object();

    private readonly IAppDbContext appDbContext;

    private readonly IWebHostEnvironment webHostEnvironment;

    private readonly AppConfig appConfig;

    private readonly ILogger<HomeController> logger;

    public HomeController(
        IAppDbContext appDbContext,
        AppConfig appConfig,
        IWebHostEnvironment webHostEnvironment,
        ILogger<HomeController> logger)
    {
        this.appDbContext = appDbContext;
        this.appConfig = appConfig;
        this.webHostEnvironment = webHostEnvironment;
        this.logger = logger;
    }

    [HttpGet]
    [Route("GetMemberInfoFromCode")]
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

    [HttpGet]
    [Route("GetPointValueFromCode")]
    public IActionResult GetPointValueFromCode(string code, string memberCode)
    {
        
        if (appDbContext.Members!.Any(x => x.Code == code))
        {
            return BadRequest("Expected coin code but received user code.");
        }

        CoinPointTranslationResult result;
        
        try
        {
            result = CodeTranslator.TranslateCoinPointCode(code);
        }
        catch (CodeTranslationException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException)
        {
            return NotFound($"A coin with the code '{code}' could not be parsed.");
        }

        var dbCoin = appDbContext.Coins!.SingleOrDefault(x => x.Code == code);

        if (dbCoin == null)
        {
            return NotFound($"A coin with the code '{code}' was not found in the database.");
        }

        var member = appDbContext.Members!.Single(x => x.Code == memberCode);
        
        if (member.Id == dbCoin.MemberId)
        {
            return base.Conflict($"The coin has already been scavenged by {member.FirstName}");
        }

        if (dbCoin.MemberId.HasValue)
        {
            var memberWhoScavengedCoin = appDbContext.Members!.Single(x => x.Id == dbCoin.MemberId);
            return base.Conflict($"The coin with code '{code}' has already been scavenged by {memberWhoScavengedCoin.FullName}!");
        }

        dbCoin.MemberId = member.Id;
        appDbContext.SaveChanges();

        return Ok(new
        {
            result.PointValue,
            result.BaseNumber,
            Code = code
        });
    }

    [HttpPost]
    [Route("AddPointsToMember")]
    public ActionResult AddPointsToMember([FromBody] PointsForMemberViewModel viewModel)
    {
        var member = appDbContext.Members!.Single(x => x.Id == viewModel.MemberId);

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
    [Route("CreateMember")]
    public object CreateMember([FromBody] CreateMemberViewModel createMemberViewModel)
    {
        lock (Locker)
        {
            var memberNumber =
                appDbContext.GenerateNextMemberCode(createMemberViewModel.TroopId, createMemberViewModel.Section);

            appDbContext.Members?.Add(new Member
            {
                Number = memberNumber,
                FirstName = createMemberViewModel.FirstName,
                LastName = createMemberViewModel.LastName,
                TroopId = createMemberViewModel.TroopId,
                SectionId = createMemberViewModel.Section,
                IsDayVisitor = createMemberViewModel.IsDayVisitor
            });

            appDbContext.SaveChanges();

            var member = appDbContext.Members!
                .Single(x => x.Number == memberNumber
                             && x.TroopId == createMemberViewModel.TroopId
                             && x.SectionId == createMemberViewModel.Section);

            return new
            {
                MemberNumber = member.Number,
                MemberID = member.Id,
            };   
        }
    }

    [HttpPut]
    [Route("UpdateMemberName")]
    public object UpdateMemberName(int memberId, string name)
    {
        var entityOrNull = appDbContext.Members!.SingleOrDefault(x => x.Id == memberId);

        if (entityOrNull != null)
        {
            entityOrNull.FirstName = name;
            // other updates here it there are any
            appDbContext.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Member id not found");
        }

        var member = appDbContext.Members!
            .Single(x => x.Id == memberId);

        return new
        {
            MemberNumber = member.Number,
            MemberID = member.Id,
        };
    }

    [HttpGet]
    [Route("GetClueStatus")]
    public object GetClueStatus(int memberId)
    {
        var member = appDbContext.Members!
            .Single(x => x.Id == memberId);

        return new
        {
            MemberId = memberId,
            member.Clue1State,
            member.Clue2State,
            member.Clue3State
        };
    }

    [HttpGet]
    [Route("GetMembers")]
    public List<Member> GetMembers()
    {
        return appDbContext.Members!.ToList();
    }

    [HttpGet]
    [Route("GetMembersWithPoints")]
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
                TotalPoints = x.ScavengeResults.SelectMany(y => y.ScavengedCoins.Select(y => y.PointValue)).Sum()
            })
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName));
    }

    [HttpGet]
    [Route("Report")]
    public ReportViewModel Report()
    {
        var top3MembersWithPointsAttached = appDbContext.GetLastThreeUsersToScanPoints()
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
                TotalPoints = x.ScavengeResults.Last().ScavengedCoins.Sum(y => y.PointValue)
            });

        var secondsUntilDeadline = appConfig.ReportDeadline > DateTime.Now
            ? (appConfig.ReportDeadline - DateTime.Now).TotalSeconds
            : 0;

        var reportViewModel = new ReportViewModel
        {
            Title = appConfig.ReportTitle,
            SecondsUntilDeadline = secondsUntilDeadline,
            ReportRefreshSeconds = appConfig.ReportRefreshSeconds,
            LastThreeUsersToScanPoints = top3MembersWithPointsAttached,
            TopThreeGroupsInLastHour = appDbContext.GetTopThreeGroupsInLastHour(),
            GroupsWithMostPointsThisWeekend = appDbContext.GetGroupsWithMostPointsThisWeekend()
        };

        return reportViewModel;
    }

    [HttpPost]
    [Route("SaveMemberPhoto")]
    public ActionResult SaveMemberPhoto([FromBody] SaveMemberPhotoViewModel saveMemberPhotoViewModel)
    {
        var convert = saveMemberPhotoViewModel.Photo.Replace("data:image/jpeg;base64,", string.Empty);
        var rootPath =
            appConfig.ContentRootDirectory
            ?? Path.Join(webHostEnvironment.ContentRootPath, "..", "woodseatsscouts.coins.web", "public", "member-images");
        var photoFileName = $"{saveMemberPhotoViewModel.MemberId}.jpg";
        var photoFullPath = Path.Join(rootPath, photoFileName);
        System.IO.File.WriteAllBytes(photoFullPath, Convert.FromBase64String(convert));
        appDbContext.Members!.Single(x => x.Id == saveMemberPhotoViewModel.MemberId).HasImage = true;
        appDbContext.SaveChanges();

        return Ok();
    }
}