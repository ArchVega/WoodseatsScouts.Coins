using Microsoft.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Tests.Integration.Helpers;

namespace WoodseatsScouts.Coins.Tests.Integration;

[Collection("Database collection")]
public class AppDbContextTests
{
    private readonly DatabaseDataHelper databaseDataHelper;
    private readonly AppDbContext appDbContext;

    public AppDbContextTests(DatabaseFixture fixture)
    {
        appDbContext = fixture!.AppDbContext;
        databaseDataHelper = new DatabaseDataHelper(fixture.AppDbContext);
    }

    [Theory]
    [InlineData(1, "A")]
    [InlineData(1, "B")]
    public void TranslatingMemberCode(int troopNumber, string sectionCode)
    {
        databaseDataHelper.CreateTroop("Troop 1");
        databaseDataHelper.CreateTroop("Troop 2");
        
        var memberNumber = appDbContext.GenerateNextMemberCode(troopNumber, sectionCode);
        memberNumber.ShouldBe(1);
        
        databaseDataHelper.CreateMember(memberNumber, troopNumber, sectionCode);
        memberNumber = appDbContext.GenerateNextMemberCode(troopNumber, sectionCode);
        memberNumber.ShouldBe(2);
        
        databaseDataHelper.CreateMember(memberNumber, troopNumber, sectionCode);
        memberNumber = appDbContext.GenerateNextMemberCode(troopNumber, sectionCode);
        memberNumber.ShouldBe(3);

        var codes = appDbContext.Members!.Select(x => x.Code).ToList();
        var distinctCodes = codes.Distinct().ToList();
        
        distinctCodes.Count.ShouldBe(codes.Count, "Codes are not all distinct");
    }

    [Fact]
    public void RetrievingSections()
    {
        var sections = appDbContext.Sections!.ToList();
        sections.Count.ShouldBe(5);
    }
    
    [Fact]
    public void Sections_InputtingSameCode_ThrowsException()
    {
        var sections = appDbContext.Sections!.Add(new Section("A", "Duplicate"));
        var exception = Should.Throw<Exception>(() => appDbContext.SaveChanges());

        const string expectedErrorMessage = "Violation of PRIMARY KEY constraint 'PK_Sections'. Cannot insert duplicate key in object 'dbo.Sections'";
        exception.InnerException!.Message.ShouldStartWith(expectedErrorMessage);
    }
    
    [Fact]
    public void Sections_MembersSectionPropertyIsMappedAsExpected()
    {
        databaseDataHelper.ResetAll();
        var troop = databaseDataHelper.CreateTroop("Troop 1");
        databaseDataHelper.CreateMember(1, troop.Id, "A");
        
        var member = appDbContext.Members!.Include(x => x.Section).First();
        member.Section.ShouldNotBeNull();
    }
}