using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Controllers;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.View;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class MembersControllerTests
{
    [Fact]
    public void GetCoin_MemberCodeSuppliedInsteadOfCoinCode_ThrowsException()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var imagePersisterMock = new Mock<IImagePersister>();
        var coinsController = new MembersController(appDbContextMock.Object, imagePersisterMock.Object);

        SetupDbMock(appDbContextMock, x => x.Members, [new Member { }]);
        SetupDbMock(appDbContextMock, x => x.ScavengeResults, [new ScavengeResult { }]);
        SetupDbMock(appDbContextMock, x => x.ScavengedCoins, [new ScavengedCoin { }]);
        SetupDbMock(appDbContextMock, x => x.Troops, [new Troop { }]);
        SetupDbMock(appDbContextMock, x => x.Sections, [new Section { }]);
        
        var results = coinsController.GetMembersWithPoints();

        results.ShouldNotBeNull();
        results.ShouldBeOfType<OkObjectResult>();
        var viewModels = (List<MembersWithPointsViewModel>)((OkObjectResult)results).Value!;
        viewModels.Count.ShouldBe(2);
    }

    private void SetupDbMock<T>(Mock<IAppDbContext> appDbContextMock, Func<IAppDbContext, DbSet<T>?> func, List<T> members) where T : class
    {
        throw new NotImplementedException();
    }
}