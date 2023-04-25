using Shouldly;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.Tests.Integration;

[Collection("Database collection")]
public class AppDbContextTests
{
    private readonly DatabaseFixture fixture;

    public AppDbContextTests(DatabaseFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData(1, "A", 1)]
    [InlineData(13, "B", 4)]
    [InlineData(17, "C", 6)]
    public void TranslatingMemberCode(int troopNumber, string section, int expectedNextMemberNumber)
    {
        fixture.AppDbContext.GenerateNextMemberCode(troopNumber, section).ShouldBe(expectedNextMemberNumber);
    }
}