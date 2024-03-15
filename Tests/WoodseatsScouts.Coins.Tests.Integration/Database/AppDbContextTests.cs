using Microsoft.EntityFrameworkCore;
using Shouldly;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Tests.Integration.Helpers;

namespace WoodseatsScouts.Coins.Tests.Integration.Database;

[Collection("Database collection")]
public class AppDbContextTests
{
    private readonly DatabaseDataHelper databaseDataHelper;
    private readonly AppDbContext appDbContext;
    private readonly DatabaseFixture fixture;

    public AppDbContextTests(DatabaseFixture fixture)
    {
        this.fixture = fixture!;
        appDbContext = fixture.AppDbContext;
        databaseDataHelper = new DatabaseDataHelper(fixture.AppDbContext);
    }

    [Theory]
    [InlineData(1, "A", 5)]
    public void GenerateNextMemberCode(int troopNumber, string sectionCode, int expectedStartingMemberNumber)
    {
        fixture.RestoreBaseTestData();
        
        var memberNumber = appDbContext.GenerateNextMemberCode(troopNumber, sectionCode);
        memberNumber.ShouldBe(expectedStartingMemberNumber);
        
        databaseDataHelper.CreateMember(memberNumber, troopNumber, sectionCode);
        memberNumber = appDbContext.GenerateNextMemberCode(troopNumber, sectionCode);
        memberNumber.ShouldBe(expectedStartingMemberNumber + 1);
        
        databaseDataHelper.CreateMember(memberNumber, troopNumber, sectionCode);
        memberNumber = appDbContext.GenerateNextMemberCode(troopNumber, sectionCode);
        memberNumber.ShouldBe(expectedStartingMemberNumber + 2);

        var codes = appDbContext.Members!.Select(x => x.Code).ToList();
        var distinctCodes = codes.Distinct().ToList();
        
        distinctCodes.Count.ShouldBe(codes.Count, "Codes are not all distinct");
    }

    [Fact]
    public void RetrievingSections()
    {
        fixture.RestoreBaseTestData();
        
        var sections = appDbContext.Sections!.ToList();
        sections.Count.ShouldBe(5);
    }
    
    [Fact]
    public void Sections_InputtingSameCode_ThrowsException()
    {
        fixture.RestoreBaseTestData();
        
        var sections = appDbContext.Sections!.Add(new Section("A", "Duplicate"));
        var exception = Should.Throw<Exception>(() => appDbContext.SaveChanges());

        const string expectedErrorMessage = "Violation of PRIMARY KEY constraint 'PK_Sections'. Cannot insert duplicate key in object 'dbo.Sections'";
        exception.InnerException!.Message.ShouldStartWith(expectedErrorMessage);
    }
    
    [Fact]
    public void Sections_MembersSectionPropertyIsMappedAsExpected()
    {
        // databaseDataHelper.ResetAll();
        var troop = databaseDataHelper.CreateTroop("Troop 1");
        databaseDataHelper.CreateMember(1, troop.Id, "A");
        
        var member = appDbContext.Members!.Include(x => x.Section).First();
        member.Section.ShouldNotBeNull();
    }
}