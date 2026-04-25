using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Controllers;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;
using WoodseatsScouts.Coins.Api.Models.Queries;
using WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Members;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class ScoutMemberControllerTests
{
    private readonly Mock<IMemberService> memberServiceMock = new();
    private readonly Mock<IAppDbContext> appDbContextMock = new();
    private readonly Mock<IImagePersister> imagePersisterMock = new();
    private readonly Mock<IOptions<AppSettings>> appSettingsOptions = new();

    private ScoutMemberController CreateCut()
    {
        return new ScoutMemberController(memberServiceMock.Object, appDbContextMock.Object, imagePersisterMock.Object, appSettingsOptions.Object);
    }

    // These need to go into a db test
    // GetMembersWithPoints_ReturnsValidViewModel
    // var scoutGroup = new ScoutGroup { Id = 1 };
    // var section = new ScoutSection { Code = "A" };
    // var scavengeResults = new List<ScanSession> { new() { ScanCoins = scavengedCoins } };
    // SetupDbMock(appDbContextMock, x => x.ScoutGroups!, [scoutGroup]);
    // SetupDbMock(appDbContextMock, x => x.ScoutSections!, [section]);
    // SetupDbMock(appDbContextMock, x => x.ScanCoins!, scavengedCoins);
    // SetupDbMock(appDbContextMock, x => x.ScanSessions!, scavengeResults);
    // SetupDbMock(appDbContextMock, x => x.ScoutMembers!, [
    //     new Api.Models.Domain.ScoutMember { ScoutGroupId = 1, ScoutGroup = scoutGroup, ScoutSectionId = "A", ScoutSection = section, ScavengeResults = scavengeResults }
    // ]);

    [Fact]
    public void GetMembersWithPoints_ReturnsValidRequestModel()
    {
        var membersController = CreateCut();

        var scavengedCoins = new List<ScanCoin>
        {
            new()
            {
                Coin = TestDataFactory.CreateCoin(13),
                ScanSession = null
            },
            new()
            {
                Coin = TestDataFactory.CreateCoin(6),
                ScanSession = null
            }
        };

        memberServiceMock.Setup(x => x.GetMemberWithPointsSummaryDtos()).Returns([
            new MemberPointsSummaryDto(new ScoutMember
            {
                ScoutGroup = TestDataFactory.CreateScoutGroup(""),
                ScoutSection = TestDataFactory.CreateScoutSection(""),
                ScanSessions =
                [
                    new ScanSession
                    {
                        ScanCoins =
                        [
                            new ScanCoin
                            {
                                Coin = TestDataFactory.CreateCoin(23),
                                ScanSession = null
                            },

                            new ScanCoin
                            {
                                Coin = TestDataFactory.CreateCoin(22),
                                ScanSession = null
                            }
                        ],
                        ScoutMember = null
                    }
                ]
            })
        ]);

        var results = membersController.GetAllScoutMembers();

        results.ShouldNotBeNull();
        results.ShouldBeOfType<OkObjectResult>();
        var memberPointsSummaryDtos = (List<MemberPointsSummaryDto>)((OkObjectResult)results).Value!;
        memberPointsSummaryDtos.Count.ShouldBe(1);

        var memberPointsSummaryDto = memberPointsSummaryDtos[0];

        memberPointsSummaryDto.TotalPoints.ShouldBe(45); // todo create test for total points calculation
    }

    [Fact]
    public void GetMemberInfoFromCode_InvalidCode_ThrowsException()
    {
        var membersController = CreateCut();

        var result = membersController.GetScoutMemberByCode("invalid", null);
        result.ShouldBeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.ShouldBe("Oops, we can't find your profile - please speak to a District Camp Leader");
    }

    [Fact]
    public void GetMemberInfoFromCode_ValidCode_OkResult()
    {
        var membersController = CreateCut();

        var result = membersController.GetScoutMemberByCode("M001A003", new Member { View = View.Basic });
        result.ShouldBeOfType<OkObjectResult>();
        memberServiceMock.Verify(x => x.GetMemberId(3, 1, "A"), Times.Once);
    }

    [Fact]
    public void SaveMemberPhoto_UpdatesHasImageProperty()
    {
        var membersController = CreateCut();

        SetupDbMock(appDbContextMock, x => x.ScoutMembers!, [
            new Api.Models.Domain.ScoutMember { Id = 9 }
        ]);

        var saveMemberPhotoViewModel = new SaveMemberPhotoRequestModel { Photo = "test-image-string" };
        var result = Should.NotThrow(() => membersController.SaveScoutMemberPhoto(9, saveMemberPhotoViewModel));

        appDbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        imagePersisterMock.Verify(x => x.Persist("9", "test-image-string"));
        result.ShouldBeOfType<OkResult>();
    }

    // [Fact] // todo: test file stream?
    // public void GetPhoto()
    // {
    //     var membersController = CreateCut();
    //
    //     membersController.Get(1);
    // }

    [Fact]
    public void AddPointsToMember_Todo1()
    {
        var membersController = CreateCut();

        var scavengedCoins = new List<ScanCoin>
        {
            new()
            {
                Coin = TestDataFactory.CreateCoin(13),
                ScanSession = null
            },
            new()
            {
                Coin = TestDataFactory.CreateCoin(6),
                ScanSession = null
            }
        };
        var scavengeResults = new List<ScanSession>
        {
            new()
            {
                ScanCoins = scavengedCoins,
                ScoutMember = null
            }
        };
        SetupDbMock(appDbContextMock, x => x.ScanCoins!, scavengedCoins);
        SetupDbMock(appDbContextMock, x => x.ScanSessions!, scavengeResults);
        SetupDbMock(appDbContextMock, x => x.ScoutMembers!, [
            new Api.Models.Domain.ScoutMember { Id = 9 }
        ]);

        appDbContextMock
            .Setup(x => x.RecordMemberAgainstUnscavengedCoins(It.IsAny<Api.Models.Domain.ScoutMember>(), It.IsAny<List<string>>()))
            .Returns(new List<Coin>());

        var pointsForMemberViewModel = new AssignCoinsToScoutMembersRequest { CoinCodes = ["C0001001010", "C0001001020"] };
        var result = Should.NotThrow(() => membersController.AssignCoinsToScoutMember(9, pointsForMemberViewModel));

        result.ShouldBeOfType<CreatedAtActionResult>();
    }

    private static void SetupDbMock<T>(
        Mock<IAppDbContext> appDbContextMock,
        Expression<Func<IAppDbContext, DbSet<T>>> expression,
        IEnumerable<T> entities)
        where T : class
    {
        appDbContextMock.Setup(expression).ReturnsDbSet((entities));
    }
}