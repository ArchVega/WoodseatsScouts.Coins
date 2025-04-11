using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Controllers;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.View;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class MembersControllerTests
{
    [Fact]
    public void GetMembersWithPoints_ReturnsValidViewModel()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var appSettingsOptionsMock = new Mock<IOptions<AppSettings>>();
        var imagePersisterMock = new Mock<IImagePersister>();

        var membersController = new MembersController(appDbContextMock.Object, appSettingsOptionsMock.Object, imagePersisterMock.Object);

        var troop = new Troop { Id = 1 };
        var section = new Section { Code = "A" };
        var scavengedCoins = new List<ScavengedCoin> { new() { PointValue = 13 }, new() { PointValue = 6 } };
        var scavengeResults = new List<ScavengeResult> { new() { ScavengedCoins = scavengedCoins } };

        SetupDbMock(appDbContextMock, x => x.Troops!, [troop]);
        SetupDbMock(appDbContextMock, x => x.Sections!, [section]);
        SetupDbMock(appDbContextMock, x => x.ScavengedCoins!, scavengedCoins);
        SetupDbMock(appDbContextMock, x => x.ScavengeResults!, scavengeResults);
        SetupDbMock(appDbContextMock, x => x.Members!, [
            new Member { TroopId = 1, Troop = troop, SectionId = "A", Section = section, ScavengeResults = scavengeResults }
        ]);

        var results = membersController.GetMembersWithPoints();

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
        var appDbContextMock = new Mock<IAppDbContext>();
        var appSettingsOptionsMock = new Mock<IOptions<AppSettings>>();
        var imagePersisterMock = new Mock<IImagePersister>();
        var membersController = new MembersController(appDbContextMock.Object, appSettingsOptionsMock.Object, imagePersisterMock.Object);

        var result = membersController.GetMemberInfoFromCode("invalid");
        result.ShouldBeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.ShouldBe("Oops, we can't find your profile - please speak to a District Camp Leader");
    }

    [Fact]
    public void GetMemberInfoFromCode_ValidCode_OkResult()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var appSettingsOptionsMock = new Mock<IOptions<AppSettings>>();
        var imagePersisterMock = new Mock<IImagePersister>();
        var membersController = new MembersController(appDbContextMock.Object, appSettingsOptionsMock.Object, imagePersisterMock.Object);

        var troop = new Troop { Id = 1 };
        var section = new Section { Code = "A" };

        SetupDbMock(appDbContextMock, x => x.Troops!, [troop]);
        SetupDbMock(appDbContextMock, x => x.Sections!, [section]);
        SetupDbMock(appDbContextMock, x => x.Members!, [
            new Member { TroopId = 1, Troop = troop, SectionId = "A", Section = section, Number = 3 }
        ]);

        var result = membersController.GetMemberInfoFromCode("M001A003");
        result.ShouldBeOfType<OkObjectResult>();
        var viewModel = (MemberViewModel)((OkObjectResult)result).Value!;
        viewModel.MemberSection.ShouldBe("A");
    }

    [Fact]
    public void SaveMemberPhoto_UpdatesHasImageProperty()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var appSettingsOptionsMock = new Mock<IOptions<AppSettings>>();
        var imagePersisterMock = new Mock<IImagePersister>();
        var membersController = new MembersController(appDbContextMock.Object, appSettingsOptionsMock.Object, imagePersisterMock.Object);

        SetupDbMock(appDbContextMock, x => x.Members!, [
            new Member { Id = 9 }
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
        var appDbContextMock = new Mock<IAppDbContext>();
        var appSettingsOptionsMock = new Mock<IOptions<AppSettings>>();
        var imagePersisterMock = new Mock<IImagePersister>();
        var membersController = new MembersController(appDbContextMock.Object, appSettingsOptionsMock.Object, imagePersisterMock.Object);

        membersController.Get(1);
    }

    [Fact]
    public void AddPointsToMember_ValidData_Ok()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var appSettingsOptionsMock = new Mock<IOptions<AppSettings>>();
        var imagePersisterMock = new Mock<IImagePersister>();
        var membersController = new MembersController(appDbContextMock.Object, appSettingsOptionsMock.Object, imagePersisterMock.Object);

        var scavengedCoins = new List<ScavengedCoin> { new() { PointValue = 13 }, new() { PointValue = 6 } };
        var scavengeResults = new List<ScavengeResult> { new() { ScavengedCoins = scavengedCoins } };
        SetupDbMock(appDbContextMock, x => x.ScavengedCoins!, scavengedCoins);
        SetupDbMock(appDbContextMock, x => x.ScavengeResults!, scavengeResults);
        SetupDbMock(appDbContextMock, x => x.Members!, [
            new Member { Id = 9 }
        ]);

        appDbContextMock
            .Setup(x => x.RecordMemberAgainstUnscavengedCoins(It.IsAny<Member>(), It.IsAny<List<string>>()))
            .Returns(new List<Coin>());

        var pointsForMemberViewModel = new PointsForMemberViewModel { CoinCodes = ["C0001001010", "C0001001020"] };
        var result = Should.NotThrow(() => membersController.AddPointsToMember(9, pointsForMemberViewModel));

        result.ShouldBeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public void AddPointsToMember_AttemptToSubmitANewHaulWithinExclusionPeriod_ThrowsException()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var appSettingsOptionsMock = new Mock<IOptions<AppSettings>>();
        var imagePersisterMock = new Mock<IImagePersister>();
        appSettingsOptionsMock.Setup(x => x.Value).Returns(new AppSettings { SecondsBetweenScavengerHauls = 10 });
        var membersController = new MembersController(appDbContextMock.Object, appSettingsOptionsMock.Object, imagePersisterMock.Object);

        var scavengedCoins = new List<ScavengedCoin> { new() { PointValue = 13 }, new() { PointValue = 6 } };
        var scavengeResults = new List<ScavengeResult>
        {
            new()
            {
                MemberId = 9,
                ScavengedCoins = scavengedCoins,
                CompletedAt = DateTime.Now
            }
        };
        SetupDbMock(appDbContextMock, x => x.ScavengedCoins!, scavengedCoins);
        SetupDbMock(appDbContextMock, x => x.ScavengeResults!, scavengeResults);
        SetupDbMock(appDbContextMock, x => x.Members!, [new Member { Id = 9 }]);

        appDbContextMock
            .Setup(x => x.RecordMemberAgainstUnscavengedCoins(It.IsAny<Member>(), It.IsAny<List<string>>()))
            .Returns(new List<Coin>());

        var pointsForMemberViewModel = new PointsForMemberViewModel { CoinCodes = ["C0001001010", "C0001001020"] };

        Should
            .Throw<ApplicationException>(() => membersController.AddPointsToMember(9, pointsForMemberViewModel))
            .Message.ShouldBe("Oops, your coins have already been stashed!");
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