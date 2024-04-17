using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Shouldly;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Tests.Integration.Helpers;

namespace WoodseatsScouts.Coins.Tests.Integration.Database;

[Collection("Database collection")]
public class AppDbContextTests(DatabaseFixture databaseFixture)
{
    private readonly DatabaseDataHelper databaseDataHelper = new(databaseFixture.AppDbContext);
    private readonly AppDbContext appDbContext = databaseFixture.AppDbContext;

    [Theory]
    [InlineData(1, "A", 5)]
    public void GenerateNextMemberCode(int troopNumber, string sectionCode, int expectedStartingMemberNumber)
    {
        databaseFixture.RestoreBaseTestData();
        
        var member = appDbContext.CreateMember("any", "any", troopNumber, sectionCode, false);
        member.Number.ShouldBe(expectedStartingMemberNumber);

        member = appDbContext.CreateMember("any", "any", troopNumber, sectionCode, false);
        member.Number.ShouldBe(expectedStartingMemberNumber + 1);
        
        member = appDbContext.CreateMember("any", "any", troopNumber, sectionCode, false);
        member.Number.ShouldBe(expectedStartingMemberNumber + 2);

        var codes = appDbContext.Members!.Select(x => x.Code).ToList();
        var distinctCodes = codes.Distinct().ToList();
        
        distinctCodes.Count.ShouldBe(codes.Count, "Codes are not all distinct");
    }

    [Fact]
    public void RetrievingSections()
    {
        databaseFixture.RestoreBaseTestData();
        
        var sections = appDbContext.Sections!.ToList();
        sections.Count.ShouldBe(5);
    }
    
    [Fact]
    public void Sections_InputtingSameCode_ThrowsException()
    {
        databaseFixture.RestoreBaseTestData();
        
        appDbContext.Sections!.Add(new Section("X", "Duplicate"));
        appDbContext.SaveChanges();
        
        appDbContext.ChangeTracker.Clear();
        
        appDbContext.Sections!.Add(new Section("X", "Duplicate"));
        var exception = Should.Throw<Exception>(() => appDbContext.SaveChanges());

        const string expectedErrorMessage = "Violation of PRIMARY KEY constraint 'PK_Sections'. Cannot insert duplicate key in object 'dbo.Sections'";
        exception.InnerException!.Message.ShouldStartWith(expectedErrorMessage);
    }
    
    [Fact]
    public void Sections_MembersSectionPropertyIsMappedAsExpected()
    {
        databaseFixture.RestoreBaseTestData();
        
        var troop = appDbContext.CreateTroop(int.MaxValue,"Troop 1");
        appDbContext.CreateMember("any", "any", troop.Id, "A", false);
        
        var member = appDbContext.Members!.Include(x => x.Section).First();
        member.Section.ShouldNotBeNull();
    }

    [Fact]
    public void UpdateMemberName()
    {
        databaseFixture.RestoreBaseTestData();

        var member = appDbContext.Members!.First();
        Should.NotThrow(() => appDbContext.UpdateMemberName(member.Id, "test-first-name", "test-last-name"));
    }
}