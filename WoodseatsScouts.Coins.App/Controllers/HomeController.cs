﻿using System.Globalization;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.App.AppLogic;
using WoodseatsScouts.Coins.App.AppLogic.Translators;
using WoodseatsScouts.Coins.App.Config;
using WoodseatsScouts.Coins.App.Data;
using WoodseatsScouts.Coins.App.Models;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IAppDbContext context;
    
    private readonly IWebHostEnvironment webHostEnvironment;

    private readonly AppConfig appConfig;
    
    private readonly ILogger<HomeController> logger;

    public HomeController(
        IAppDbContext context,
        AppConfig appConfig,
        IWebHostEnvironment webHostEnvironment,
        ILogger<HomeController> logger)
    {
        this.context = context;
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
            var member = context.Members!
                .Single(x => x.Number == translationResult.MemberNumber
                             && x.TroopId == translationResult.TroopNumber
                             && x.Section == translationResult.Section);

            // The QRScanner for coins becomes active after 500ms after a member has logged in. Slight delay to allow the admin to shift focus away.
            Thread.Sleep(2000);
            
            return Ok(new
            {
                member.FirstName,
                member.LastName,
                MemberPhotoPath = $"/member-images/{member.Id}.jpg",
                MemberTroopNumber = member.TroopId,
                MemberSection = member.Section,
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
    public IActionResult GetPointValueFromCode(string code)
    {
        if (context.Members!.Any(x => x.Code == code))
        {
            return BadRequest("Expected coin code but received user code.");
        }
        
        var result = CodeTranslator.TranslateCoinPointCode(code);
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
        var member = context.Members!.Single(x => x.Id == viewModel.MemberId);

        var tallyHistoryItem = new ScavengeResult()
        {
            MemberId = member.Id,
            CompletedAt = DateTime.Now
        };

        context.ScavengeResults!.Add(tallyHistoryItem);

        context.SaveChanges();

        foreach (var coinCode in viewModel.CoinCodes)
        {
            var result = CodeTranslator.TranslateCoinPointCode(coinCode);

            context.ScavengedCoins?.Add(new ScavengedCoin
            {
                ScavengeResultId = tallyHistoryItem.Id,
                BaseNumber = result.BaseNumber,
                PointValue = result.PointValue,
                Code = coinCode
            });
        }

        context.SaveChanges();

        return CreatedAtAction(nameof(AddPointsToMember), null, null);
    }

    [HttpPost]
    [Route("CreateMember")]
    public object CreateMember([FromBody] CreateMemberViewModel createMemberViewModel)
    {
        var memberNumber =
            context.GenerateNextMemberCode(createMemberViewModel.TroopId, createMemberViewModel.Section);
   
        context.Members?.Add(new Member
        {
            Number = memberNumber,
            FirstName = createMemberViewModel.Name,
            TroopId = createMemberViewModel.TroopId,
            Section = createMemberViewModel.Section
        });

        context.SaveChanges();

        var member = context.Members!
            .Single(x => x.Number == memberNumber
                         && x.TroopId == createMemberViewModel.TroopId
                         && x.Section == createMemberViewModel.Section);

        return new
        {
            MemberNumber = member.Number,
            MemberID = member.Id,
        };
    }

    [HttpPut]
    [Route("UpdateMemberName")]
    public object UpdateMemberName(int memberId, string name)
    {
        var entityOrNull = context.Members!.SingleOrDefault(x => x.Id == memberId);

        if (entityOrNull != null)
        {
            entityOrNull.FirstName = name;
            // other updates here it there are any
            context.SaveChanges();
        }
        else
        {
            throw new ArgumentException("Member id not found");
        }

        var member = context.Members!
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
        var member = context.Members!
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
        return context.Members!.ToList();
    }
    
    [HttpGet]
    [Route("GetMembersWithPoints")]
    public OkObjectResult GetMembersWithPoints()
    {
        return Ok(context.Members!
            .Include(x => x.ScavengeResults)
            .ThenInclude(x => x.ScavengedCoins)
            .Include(x => x.Troop)
            .Select(x => new
            {
                x.Id,
                MemberCode = x.Code,
                x.HasImage,
                MemberNumber = x.Number,
                x.FirstName,
                x.LastName,
                TroopName = x.Troop.Name,
                x.Section,
                TotalPoints = x.ScavengeResults.SelectMany(x => x.ScavengedCoins.Select(y => y.PointValue)).Sum()
            })
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName));
    }

    [HttpGet]
    [Route("Report")]
    public ReportViewModel Report()
    {
        var top3Members = context.GetLastThreeUsersToScanPoints();
        var top3MemberIds = top3Members.Select(x => x.Id).ToList();
        var top3MembersWithPointsAttached
            = context.Members!
                .Where(x =>  top3MemberIds.Contains(x.Id))
                .Include(x => x.ScavengeResults)
                .ThenInclude(x => x.ScavengedCoins)
                .Include(x => x.Troop)
                .Select(x => new
                {
                    x.Id,
                    MemberCode = x.Code,
                    x.HasImage,
                    MemberNumber = x.Number,
                    x.FirstName,
                    x.LastName,
                    TroopName = x.Troop.Name,
                    x.Section,
                    TotalPoints = x.ScavengeResults.SelectMany(x => x.ScavengedCoins.Select(y => y.PointValue)).Sum()
                }).ToList();

        
        var secondsUntilDeadline = appConfig.ReportDeadline > DateTime.Now ? (appConfig.ReportDeadline - DateTime.Now).TotalSeconds : 0;
        
        var reportViewModel = new ReportViewModel
        {
            Title = appConfig.ReportTitle,
            SecondsUntilDeadline = secondsUntilDeadline,
            LastThreeUsersToScanPoints = top3MembersWithPointsAttached,
            TopThreeGroupsInLastHour = context.GetTopThreeGroupsInLastHour(),
            GroupsWithMostPointsThisWeekend = context.GetGroupsWithMostPointsThisWeekend()
        };

        return reportViewModel;
    }

    [HttpPost]
    [Route("SaveMemberPhoto")]
    public ActionResult SaveMemberPhoto([FromBody] SaveMemberPhotoViewModel saveMemberPhotoViewModel)
    {
        var convert = saveMemberPhotoViewModel.Photo.Replace("data:image/jpeg;base64,", string.Empty);
        var rootPath = webHostEnvironment.ContentRootPath;
        var memberImagesDirectoryPath = Path.Join(rootPath, "ClientApp", "public", "member-images");
        var photoFileName = $"{saveMemberPhotoViewModel.MemberId}.jpg";
        var photoFullPath = Path.Join(memberImagesDirectoryPath, photoFileName);
        System.IO.File.WriteAllBytes(photoFullPath, Convert.FromBase64String(convert));

        return Ok();
    }
}