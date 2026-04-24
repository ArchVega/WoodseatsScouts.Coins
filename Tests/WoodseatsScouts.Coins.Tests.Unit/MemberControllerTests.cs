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
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;
using WoodseatsScouts.Coins.Api.Models.Queries;
using WoodseatsScouts.Coins.Api.Models.View;
using WoodseatsScouts.Coins.Api.Models.View.Members;
using Xunit;
using Member = WoodseatsScouts.Coins.Api.Models.Queries.Member;

namespace WoodseatsScouts.Coins.Tests;

public class MemberControllerTests
{
    private readonly Mock<IMemberService> memberServiceMock = new();
    private readonly Mock<IAppDbContext> appDbContextMock = new();
    private readonly Mock<IImagePersister> imagePersisterMock = new();
    private readonly Mock<IOptions<LeaderboardSettings>> leaderboardSettingsOptions = new();
    private readonly Mock<IOptions<AppSettings>> appSettingsOptions = new();

    private MemberController CreateCut()
    {
        return new MemberController(memberServiceMock.Object, appDbContextMock.Object, imagePersisterMock.Object, appSettingsOptions.Object, leaderboardSettingsOptions.Object);
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
    public void GetMembersWithPoints_ReturnsValidViewModel()
    {
        var membersController = CreateCut();

        var scavengedCoins = new List<ScanCoin> { new() { Coin = new Coin() { Value = 13 } }, new() { Coin = new Coin() { Value = 6 } } };

        memberServiceMock.Setup(x => x.GetMemberWithPointsSummaryDtos()).Returns([
            new MemberPointsSummaryDto(new ScoutMember
            {
                ScoutGroup = new ScoutGroup(),
                ScoutSection = new ScoutSection(),
                ScavengeResults =
                [
                    new ScanSession
                    {
                        ScanCoins =
                        [
                            new ScanCoin
                            {
                                Coin = new Coin
                                {
                                    Value = 23
                                }
                            },

                            new ScanCoin
                            {
                                Coin = new Coin
                                {
                                    Value = 22
                                }
                            }
                        ]
                    }
                ]
            })
        ]);

        var results = membersController.GetAllMembers(new Member() { View = View.PointsSummary });

        results.ShouldNotBeNull();
        results.ShouldBeOfType<OkObjectResult>();
        var viewModels = (List<MemberPointsSummaryDto>)((OkObjectResult)results).Value!;
        viewModels.Count.ShouldBe(1);

        var membersWithPointsViewModel = viewModels[0];

        membersWithPointsViewModel.TotalPoints.ShouldBe(45); // todo create test for total points calculation
    }

    [Fact]
    public void GetMemberInfoFromCode_InvalidCode_ThrowsException()
    {
        var membersController = CreateCut();
    
        var result = membersController.GetMemberByCode("invalid", null);
        result.ShouldBeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.ShouldBe("Oops, we can't find your profile - please speak to a District Camp Leader");
    }

    [Fact]
    public void GetMemberInfoFromCode_ValidCode_OkResult()
    {
        var membersController = CreateCut();

        var result = membersController.GetMemberByCode("M001A003", new Member { View = View.Basic });
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

        var saveMemberPhotoViewModel = new SaveMemberPhotoViewModel { Photo = "test-image-string" };
        var result = Should.NotThrow(() => membersController.SaveMemberPhoto(9, saveMemberPhotoViewModel));

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

        var scavengedCoins = new List<ScanCoin> { new() { Coin = new Coin() { Value = 13 } }, new() { Coin = new Coin() { Value = 6 } } };
        var scavengeResults = new List<ScanSession> { new() { ScanCoins = scavengedCoins } };
        SetupDbMock(appDbContextMock, x => x.ScanCoins!, scavengedCoins);
        SetupDbMock(appDbContextMock, x => x.ScanSessions!, scavengeResults);
        SetupDbMock(appDbContextMock, x => x.ScoutMembers!, [
            new Api.Models.Domain.ScoutMember { Id = 9 }
        ]);

        appDbContextMock
            .Setup(x => x.RecordMemberAgainstUnscavengedCoins(It.IsAny<Api.Models.Domain.ScoutMember>(), It.IsAny<List<string>>()))
            .Returns(new List<Coin>());

        var pointsForMemberViewModel = new PointsForMemberViewModel { CoinCodes = ["C0001001010", "C0001001020"] };
        var result = Should.NotThrow(() => membersController.AddPointsToMember(9, pointsForMemberViewModel));

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