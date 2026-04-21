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

    [Fact]
    public void GetMembersWithPoints_ReturnsValidViewModel()
    {
        var membersController = CreateCut();

        var scoutGroup = new ScoutGroup { Id = 1 };
        var section = new Section { Code = "A" };
        var scavengedCoins = new List<ScavengedCoin> { new() { Coin = new Coin() { Value = 13 } }, new() { Coin = new Coin() { Value = 6}} }; // changed
        var scavengeResults = new List<ScavengeResult> { new() { ScavengedCoins = scavengedCoins } };

        SetupDbMock(appDbContextMock, x => x.ScoutGroups!, [scoutGroup]);
        SetupDbMock(appDbContextMock, x => x.Sections!, [section]);
        SetupDbMock(appDbContextMock, x => x.ScavengedCoins!, scavengedCoins);
        SetupDbMock(appDbContextMock, x => x.ScavengeResults!, scavengeResults);
        SetupDbMock(appDbContextMock, x => x.Members!, [
            new Api.Models.Domain.Member { ScoutGroupId = 1, ScoutGroup = scoutGroup, SectionId = "A", Section = section, ScavengeResults = scavengeResults }
        ]);

        var results = membersController.GetAllMembers(null);

        results.ShouldNotBeNull();
        results.ShouldBeOfType<OkObjectResult>();
        var viewModels = (List<MembersWithPointsViewModel>)((OkObjectResult)results).Value!;
        viewModels.Count.ShouldBe(1);

        var membersWithPointsViewModel = viewModels[0];

        membersWithPointsViewModel.TotalPoints.ShouldBe(19);
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

        SetupDbMock(appDbContextMock, x => x.Members!, [
            new Api.Models.Domain.Member { Id = 9 }
        ]);

        var saveMemberPhotoViewModel = new SaveMemberPhotoViewModel { Photo = "test-image-string" };
        var result = Should.NotThrow(() => membersController.SaveMemberPhoto(9, saveMemberPhotoViewModel));

        appDbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        imagePersisterMock.Verify(x => x.Persist("9", "test-image-string"));
        result.ShouldBeOfType<OkResult>();
    }

    [Fact]
    public void GetPhoto()
    {
        var membersController = CreateCut();

        membersController.Get(1);
    }

    [Fact]
    public void AddPointsToMember_Todo1()
    {
        var membersController = CreateCut();

        // var scavengedCoins = new List<ScavengedCoin> { new() { PointValue = 13 }, new() { PointValue = 6 } };
        var scavengedCoins = new List<ScavengedCoin> { new() { Coin = new Coin() { Value = 13 } }, new() { Coin = new Coin() { Value = 6}} }; // changed
        var scavengeResults = new List<ScavengeResult> { new() { ScavengedCoins = scavengedCoins } };
        SetupDbMock(appDbContextMock, x => x.ScavengedCoins!, scavengedCoins);
        SetupDbMock(appDbContextMock, x => x.ScavengeResults!, scavengeResults);
        SetupDbMock(appDbContextMock, x => x.Members!, [
            new Api.Models.Domain.Member { Id = 9 }
        ]);

        appDbContextMock
            .Setup(x => x.RecordMemberAgainstUnscavengedCoins(It.IsAny<Api.Models.Domain.Member>(), It.IsAny<List<string>>()))
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