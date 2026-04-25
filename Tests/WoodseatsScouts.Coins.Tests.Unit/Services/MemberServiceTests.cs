using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Queries;
using WoodseatsScouts.Coins.Api.Services;
using Xunit;

namespace WoodseatsScouts.Coins.Tests.Services;

public class MemberServiceTests
{
    private readonly Mock<IAppDbContext> appDbContextMock = new();
    
    private MemberService CreateCut()
    {
        return new MemberService(appDbContextMock.Object);
    }
    
    // Obsolete
    // [Fact]
    // public void GetMemberInfoFromCode_ValidCode_OkResult()
    // {
    //     var cut = CreateCut();
    //
    //     const int memberNumber = 3;
    //     var scoutGroupId = new ScoutGroup { Id = 1 };
    //     var section = new Section { Code = "A" };
    //
    //     SetupDbMock(appDbContextMock, x => x.ScoutGroups!, [scoutGroupId]);
    //     SetupDbMock(appDbContextMock, x => x.Sections!, [section]);
    //     SetupDbMock(appDbContextMock, x => x.Members!, [new Member { ScoutGroupId = 1, ScoutGroup = scoutGroupId, SectionId = "A", Section = section, Number = memberNumber }]);
    //
    //     var result = cut.GetMemberIdFromFragments(memberNumber, scoutGroupId.Id, section.Code);
    //     result.Section.Code.ShouldBe("A");
    // }

    private static void SetupDbMock<T>(
        Mock<IAppDbContext> appDbContextMock,
        Expression<Func<IAppDbContext, DbSet<T>>> expression,
        IEnumerable<T> entities)
        where T : class
    {
        appDbContextMock.Setup(expression).ReturnsDbSet((entities));
    }   
}