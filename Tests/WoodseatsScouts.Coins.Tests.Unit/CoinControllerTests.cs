using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Controllers;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Api.Services;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class CoinControllerTests
{
    private readonly Mock<IAppDbContext> appDbContextMock = new();
    private readonly Mock<SystemDateTimeProvider> systemDateTimeProviderMock = new();
    private readonly Mock<ICoinService> coinServiceMock = new();

    private CoinController CreateCut()
    {
        return new CoinController(appDbContextMock.Object, coinServiceMock.Object, systemDateTimeProviderMock.Object);
    }
    
    [Fact]
    public void GetCoin_MemberCodeSuppliedInsteadOfCoinCode_ThrowsException()
    {
        var coinsController = CreateCut();
        const string memberCode = "M045B019";

        appDbContextMock.Setup(x => x.ScoutMembers).ReturnsDbSet((new List<ScoutMember> { new ScoutMember { Code = memberCode } }));

        var exception = Should.Throw<CodeTranslationException>(() => coinsController.AssignCoinToScoutMember(memberCode, It.IsAny<string>()));

        exception.Message.ShouldBe("The code 'M045B019' is a Member code");
    }

    [Fact]
    public void GetCoin_InvalidCoinCode_ThrowsException()
    {
        var coinsController = CreateCut();
        const string memberCode = "test-any-member-code";
        const string coinCode = "test-any-coin-code";

        appDbContextMock.Setup(x => x.ScoutMembers).ReturnsDbSet((new List<ScoutMember> { new ScoutMember { Code = memberCode } }));

        var exception = Should.Throw<CodeTranslationException>(() => coinsController.AssignCoinToScoutMember(coinCode, memberCode));

        exception.Message.ShouldBe("Oops, we couldn't find that coin - please speak to a District Camp Leader");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeButNotFound_ThrowsException()
    {
        var coinsController = CreateCut();
        const string memberCode = "test-valid-member-code";
        const string coinCode = "C9999999999";

        appDbContextMock.Setup(x => x.ScoutMembers).ReturnsDbSet((new List<ScoutMember> { new ScoutMember { Code = memberCode } }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin>()));

        var result = coinsController.AssignCoinToScoutMember(coinCode, memberCode);

        result.ShouldBeOfType<NotFoundObjectResult>();
        ((NotFoundObjectResult)result).Value.ShouldBe("A coin with the code 'C9999999999' was not found in the database.");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeButSuppliedMemberCodeDoesNotMatchAnyMember_ThrowsException()
    {
        var coinsController = CreateCut();
        const string memberCode = "M001A001";
        const string coinCode = "C0001010020";

        appDbContextMock.Setup(x => x.ScoutMembers).ReturnsDbSet((new List<ScoutMember> { new ScoutMember() }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin> { new Coin
            {
                Code = coinCode,
                ActivityBase = null
            }
        }));

        var result = coinsController.AssignCoinToScoutMember(coinCode, memberCode);

        result.ShouldBeOfType<NotFoundObjectResult>();
        ((NotFoundObjectResult)result).Value.ShouldBe("A member with the code 'C0001010020' was not found in the database.");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeButAlreadyRecordedByTheCurrentMember_ThrowsException()
    {
        var coinsController = CreateCut();
        const string memberCode = "M001A001";
        const string coinCode = "C0001010020";
        const int memberId = 1;
        var member = new ScoutMember { Code = memberCode, Id = memberId, FirstName = "test-first-name" };
        appDbContextMock.Setup(x => x.ScoutMembers).ReturnsDbSet((new List<ScoutMember> { member }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin>
        {
            new()
            {
                Code = coinCode,
                MemberId = memberId,
                LockUntil = DateTime.UtcNow.AddHours(1),
                ActivityBase = null
            }
        }));

        var result = coinsController.AssignCoinToScoutMember(coinCode, memberCode);

        result.ShouldBeOfType<ConflictObjectResult>();
        ((ConflictObjectResult)result).Value.ShouldBe("The coin has already been scavenged by test-first-name");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeButAlreadyScavengedByAnotherMember_ThrowsException()
    {
        var coinsController = CreateCut();
        const string memberCode = "M001A001";
        const string coinCode = "C0001010020";
        const int memberId = 1;
        var currentMember = new ScoutMember { Code = memberCode, Id = memberId, FirstName = "test-first-name" };

        const int otherMemberId = 2;
        var otherMember = new ScoutMember { Id = otherMemberId, FirstName = "test-other-first-name", LastName = "test-other-last-name" };

        appDbContextMock.Setup(x => x.ScoutMembers).ReturnsDbSet((new List<ScoutMember> { currentMember, otherMember }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin>
        {
            new()
            {
                Code = coinCode,
                MemberId = otherMemberId,
                LockUntil = DateTime.UtcNow.AddHours(1),
                ActivityBase = null
            }
        }));

        var result = coinsController.AssignCoinToScoutMember(coinCode, memberCode);

        result.ShouldBeOfType<ConflictObjectResult>();
        ((ConflictObjectResult)result).Value.ShouldBe(
            "This points token has already been used by test-other-first-name test-other-last-name. Please hand it to a District Camp Leader");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeAndNotAlreadyScavengedByAnotherMember_ReturnsCode()
    {
        var coinsController = CreateCut();
        const string memberCode = "M001A001";
        const string coinCode = "C0001010020";
        const int memberId = 1;
        var currentMember = new ScoutMember { Code = memberCode, Id = memberId, FirstName = "test-first-name" };

        appDbContextMock.Setup(x => x.ScoutMembers).ReturnsDbSet((new List<ScoutMember> { currentMember }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin> { new()
            {
                Code = coinCode,
                ActivityBase = null
            }
        }));

        var result = Should.NotThrow(() => coinsController.AssignCoinToScoutMember(coinCode, memberCode));

        result.ShouldBeOfType<OkObjectResult>();
        var coin = (CoinDto)((OkObjectResult)result).Value!;
        coin.PointValue.ShouldBe(20);
        coin.ActivityBaseId.ShouldBe(10);
        coin.Code.ShouldBe("C0001010020");
    }
}