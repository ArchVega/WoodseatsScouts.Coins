using Shouldly;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class CodeTranslatorTests
{
    [Theory]
    [InlineData("does-not-exist")]
    [InlineData("")]
    public void TranslateCoinPointCode_InvalidCode_ThrowsException(string code)
    {
        var exception = Should.Throw<CodeTranslationException>(() => CodeTranslator.TranslateCoinCode(code));
        exception.Message.ShouldBe($"Could not translate Coin Code '{code}'");
    }
    
    [Theory]
    [InlineData("does-not-exist")]
    [InlineData("")]
    public void TranslateMemberCode_InvalidCode_ThrowsException(string code)
    {
        var exception = Should.Throw<CodeTranslationException>(() => CodeTranslator.TranslateMemberCode(code));
        exception.Message.ShouldBe($"Could not translate Member Code '{code}'");
    }
    
    [Fact]
    public void TranslateMemberCode_CoinCodeGiven_ThrowsException()
    {
        const string validCoinCode = "B0002001020";
        var exception = Should.Throw<CodeTranslationException>(() => CodeTranslator.TranslateMemberCode(validCoinCode));
        exception.Message.ShouldBe($"The code 'B0002001020' is a Coin code");
    }
    
    [Fact]
    public void TranslateCodeCode_MemberCodeGiven_ThrowsException()
    {
        const string validCoinCode = "M001A001";
        var exception = Should.Throw<CodeTranslationException>(() => CodeTranslator.TranslateCoinCode(validCoinCode));
        exception.Message.ShouldBe($"The code 'M001A001' is a Member code");
    }

    [Theory]
    [InlineData("B0001001010",  1, "C", 1, 10)]
    [InlineData("B0047010020", 47, "C", 10, 20)]
    public void TranslatingPoints(string code, int id, string tokenIdentifier, int baseNumber, int pointValue)
    {   
        var pointTranslationResult = CodeTranslator.TranslateCoinCode(code);
        pointTranslationResult.Id.ShouldBe(id);
        pointTranslationResult.TokenIdentifier.ShouldBe(tokenIdentifier);
        pointTranslationResult.BaseNumber.ShouldBe(baseNumber);
        pointTranslationResult.PointValue.ShouldBe(pointValue);
    }
    
    [Theory]
    [InlineData("M007B004", "M", 7, "C", 4)]
    [InlineData("M023B005", "M", 23, "C", 5)]
    [InlineData("M045B008", "M", 45, "C", 8)]
    [InlineData("M045B010", "M", 45, "C", 10)]
    [InlineData("M045B019", "M", 45, "C", 19)]
    public void TranslatingMemberCode(string code, string tokenIdentifier, int troop, string section, int memberNumber)
    {
        var memberCodeTranslationResult = CodeTranslator.TranslateMemberCode(code);
        
        memberCodeTranslationResult.TokenIdentifier.ShouldBe(tokenIdentifier);
        memberCodeTranslationResult.Section.ShouldBe(section);
        memberCodeTranslationResult.TroopNumber.ShouldBe(troop);
        memberCodeTranslationResult.MemberNumber.ShouldBe(memberNumber);
    }
}