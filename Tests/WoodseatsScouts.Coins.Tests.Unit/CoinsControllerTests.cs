using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.Controllers;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class CoinsControllerTests
{
    [Fact]
    public void GetCoin_MemberCodeSuppliedInsteadOfCoinCode_ThrowsException()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var coinsController = new CoinsController(appDbContextMock.Object);
        const string memberCode = "test-member-code";

        appDbContextMock.Setup(x => x.Members).ReturnsDbSet((new List<Member> { new Member { Code = memberCode } }));

        var result = coinsController.GetCoin(memberCode, It.IsAny<string>());

        result.ShouldBeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.ShouldBe("Expected coin code but received user code.");
    }

    [Fact]
    public void GetCoin_InvalidCoinCode_ThrowsException()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var coinsController = new CoinsController(appDbContextMock.Object);
        const string memberCode = "test-valid-member-code";
        const string coinCode = "test-invalid-coin-code";

        appDbContextMock.Setup(x => x.Members).ReturnsDbSet((new List<Member> { new Member { Code = memberCode } }));

        var result = coinsController.GetCoin(coinCode, memberCode);

        result.ShouldBeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.ShouldBe("Could not translate Coin Code 'test-invalid-coin-code'");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeButNotFound_ThrowsException()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var coinsController = new CoinsController(appDbContextMock.Object);
        const string memberCode = "test-valid-member-code";
        const string coinCode = "B9999999999";

        appDbContextMock.Setup(x => x.Members).ReturnsDbSet((new List<Member> { new Member { Code = memberCode } }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin>()));

        var result = coinsController.GetCoin(coinCode, memberCode);

        result.ShouldBeOfType<NotFoundObjectResult>();
        ((NotFoundObjectResult)result).Value.ShouldBe("A coin with the code 'B9999999999' was not found in the database.");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeButSuppliedMemberCodeDoesNotMatchAnyMember_ThrowsException()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var coinsController = new CoinsController(appDbContextMock.Object);
        const string memberCode = "M001A001";
        const string coinCode = "B0001010020";

        appDbContextMock.Setup(x => x.Members).ReturnsDbSet((new List<Member> { new Member() }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin> { new Coin() { Code = coinCode } }));

        var result = coinsController.GetCoin(coinCode, memberCode);

        result.ShouldBeOfType<NotFoundObjectResult>();
        ((NotFoundObjectResult)result).Value.ShouldBe("A member with the code 'B0001010020' was not found in the database.");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeButAlreadyRecordedByTheCurrentMember_ThrowsException()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var coinsController = new CoinsController(appDbContextMock.Object);
        const string memberCode = "M001A001";
        const string coinCode = "B0001010020";
        const int memberId = 1;
        var member = new Member { Code = memberCode, Id = memberId, FirstName = "test-first-name" };
        appDbContextMock.Setup(x => x.Members).ReturnsDbSet((new List<Member> { member }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin> { new Coin() { Code = coinCode, MemberId = memberId } }));

        var result = coinsController.GetCoin(coinCode, memberCode);

        result.ShouldBeOfType<ConflictObjectResult>();
        ((ConflictObjectResult)result).Value.ShouldBe("The coin has already been scavenged by test-first-name");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeButAlreadyScavengedByAnotherMember_ThrowsException()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var coinsController = new CoinsController(appDbContextMock.Object);
        const string memberCode = "M001A001";
        const string coinCode = "B0001010020";
        const int memberId = 1;
        var currentMember = new Member { Code = memberCode, Id = memberId, FirstName = "test-first-name" };

        const int otherMemberId = 2;
        var otherMember = new Member { Id = otherMemberId, FirstName = "test-other-first-name", LastName = "test-other-last-name" };

        appDbContextMock.Setup(x => x.Members).ReturnsDbSet((new List<Member> { currentMember, otherMember }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin> { new Coin() { Code = coinCode, MemberId = otherMemberId } }));

        var result = coinsController.GetCoin(coinCode, memberCode);

        result.ShouldBeOfType<ConflictObjectResult>();
        ((ConflictObjectResult)result).Value.ShouldBe(
            "The coin with code 'B0001010020' has already been scavenged by test-other-first-name test-other-last-name!");
    }

    [Fact]
    public void GetCoin_ValidCoinCodeAndNotAlreadyScavengedByAnotherMember_ReturnsCode()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var coinsController = new CoinsController(appDbContextMock.Object);
        const string memberCode = "M001A001";
        const string coinCode = "B0001010020";
        const int memberId = 1;
        var currentMember = new Member { Code = memberCode, Id = memberId, FirstName = "test-first-name" };

        appDbContextMock.Setup(x => x.Members).ReturnsDbSet((new List<Member> { currentMember }));
        appDbContextMock.Setup(x => x.Coins).ReturnsDbSet((new List<Coin> { new() { Code = coinCode } }));

        var result = Should.NotThrow(() => coinsController.GetCoin(coinCode, memberCode));
        
        result.ShouldBeOfType<OkObjectResult>();
        var coin = (CoinViewModel)((OkObjectResult)result).Value!;
        coin.PointValue.ShouldBe(20);
        coin.BaseNumber.ShouldBe(10);
        coin.Code.ShouldBe("B0001010020");
    }
}