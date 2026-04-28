// using Microsoft.EntityFrameworkCore;
// using Shouldly;
// using WoodseatsScouts.Coins.Api.Data;
// using WoodseatsScouts.Coins.Api.Models.Domain;
// using WoodseatsScouts.Coins.Tests.Integration.Helpers;
//
// namespace WoodseatsScouts.Coins.Tests.Integration.Database;
//
// [Collection("Database collection")]
// public class AppDbContextTests(DatabaseFixture databaseFixture)
// {
//     private readonly DatabaseDataHelper databaseDataHelper = new(databaseFixture.AppDbContext);
//     private readonly AppDbContext appDbContext = databaseFixture.AppDbContext;
//
//     [Theory]
//     [InlineData(1, "A", 5)]
//     public void GenerateNextMemberCode(int scoutGroupNumber, string sectionCode, int expectedStartingMemberNumber)
//     {
//         databaseFixture.RestoreBaseTestData();
//         
//         var member = appDbContext.CreateMember("any", "any", scoutGroupNumber, sectionCode, false);
//         member.Number.ShouldBe(expectedStartingMemberNumber);
//
//         member = appDbContext.CreateMember("any", "any", scoutGroupNumber, sectionCode, false);
//         member.Number.ShouldBe(expectedStartingMemberNumber + 1);
//         
//         member = appDbContext.CreateMember("any", "any", scoutGroupNumber, sectionCode, false);
//         member.Number.ShouldBe(expectedStartingMemberNumber + 2);
//
//         var codes = appDbContext.ScoutMembers!.Select(x => x.Code).ToList();
//         var distinctCodes = codes.Distinct().ToList();
//         
//         distinctCodes.Count.ShouldBe(codes.Count, "Codes are not all distinct");
//     }
//
//     [Fact]
//     public void RetrievingSections()
//     {
//         databaseFixture.RestoreBaseTestData();
//         
//         var sections = appDbContext.ScoutSections!.ToList();
//         sections.Count.ShouldBe(5);
//     }
//     
//     [Fact]
//     public void Sections_InputtingSameCode_ThrowsException()
//     {
//         databaseFixture.RestoreBaseTestData();
//         
//         appDbContext.ScoutSections!.Add(new ScoutSection
//         {
//             Code = "X",
//             Name =  "Duplicate"
//         });
//         appDbContext.SaveChanges();
//         
//         appDbContext.ChangeTracker.Clear();
//         
//         appDbContext.ScoutSections!.Add(new ScoutSection
//         {
//             Code = "X",
//             Name = "Duplicate"
//         });
//         var exception = Should.Throw<Exception>(() => appDbContext.SaveChanges());
//
//         const string expectedErrorMessage = "Violation of PRIMARY KEY constraint 'PK_Sections'. Cannot insert duplicate key in object 'dbo.Sections'";
//         exception.InnerException!.Message.ShouldStartWith(expectedErrorMessage);
//     }
//     
//     [Fact]
//     public void Sections_MembersSectionPropertyIsMappedAsExpected()
//     {
//         databaseFixture.RestoreBaseTestData();
//         
//         var scoutGroup = appDbContext.CreateScoutGroup("ScoutGroup 1");
//         appDbContext.CreateMember("any", "any", scoutGroup.Id, "A", false);
//         
//         var member = appDbContext.ScoutMembers!.Include(x => x.ScoutSection).First();
//         member.ScoutSection.ShouldNotBeNull();
//     }
//
//     [Fact]
//     public void UpdateMemberName()
//     {
//         databaseFixture.RestoreBaseTestData();
//
//         var member = appDbContext.ScoutMembers!.First();
//         Should.NotThrow(() => appDbContext.UpdateMemberName(member.Id, "test-first-name", "test-last-name"));
//     }
// }