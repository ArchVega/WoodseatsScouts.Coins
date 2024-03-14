using Shouldly;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class CodeTranslatorTests
{
    [Fact]
    public void TranslateCoinPointCode_InvalidCode_ThrowsException()
    {
        var exception = Should.Throw<CodeTranslationException>(() => CodeTranslator.TranslateCoinPointCode("does-not-exist"));
        exception.Message.ShouldBe("Could not translate Coin Code 'does-not-exist'");
    }
    
    [Fact]
    public void TranslateMemberCode_InvalidCode_ThrowsException()
    {
        var exception = Should.Throw<CodeTranslationException>(() => CodeTranslator.TranslateMemberCode("does-not-exist"));
        exception.Message.ShouldBe("Could not translate Member Code 'does-not-exist'");
    }
    
    [Theory]
    [InlineData("B0001001010",  1, "B", 1, 10)]
    [InlineData("B0047010020", 47, "B", 10, 20)]
    public void TranslatingPoints(string code, int id, string tokenIdentifier, int baseNumber, int pointValue)
    {   
        var pointTranslationResult = CodeTranslator.TranslateCoinPointCode(code);
        pointTranslationResult.Id.ShouldBe(id);
        pointTranslationResult.TokenIdentifier.ShouldBe(tokenIdentifier);
        pointTranslationResult.BaseNumber.ShouldBe(baseNumber);
        pointTranslationResult.PointValue.ShouldBe(pointValue);
    }
    
    [Theory]
    [InlineData("M007B004", "M", 7, "B", 4)]
    [InlineData("M023B005", "M", 23, "B", 5)]
    [InlineData("M045B008", "M", 45, "B", 8)]
    [InlineData("M045B010", "M", 45, "B", 10)]
    [InlineData("M045B019", "M", 45, "B", 19)]
    public void TranslatingMemberCode(string code, string tokenIdentifier, int troop, string section, int memberNumber)
    {
        var memberCodeTranslationResult = CodeTranslator.TranslateMemberCode(code);
        
        memberCodeTranslationResult.TokenIdentifier.ShouldBe(tokenIdentifier);
        memberCodeTranslationResult.Section.ShouldBe(section);
        memberCodeTranslationResult.TroopNumber.ShouldBe(troop);
        memberCodeTranslationResult.MemberNumber.ShouldBe(memberNumber);
    }
}