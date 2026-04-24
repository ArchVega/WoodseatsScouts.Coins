using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Controllers;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.View;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class AdminControllerTests
{
    [Fact]
    public void CreateCoins_PointsNotProvided_ReturnsBadRequest()
    {
        var appDbContextMock = new Mock<IAppDbContext>();
        var adminController = new AdminController(appDbContextMock.Object);

        var result = adminController.CreateCoins(new CreateCoinViewModel
        {
            BaseId = 1,
            Count = 1
        });
        
        result.ShouldBeOfType<BadRequestObjectResult>();
        var message = ((BadRequestObjectResult)result).Value!;

        message.ToString().ShouldBe("Points must be provided.");
    }
}