using Shouldly;
using WoodseatsScouts.Coins.App.AppLogic.Translators;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class CodeTranslatorTests
{
    [Theory]
    [InlineData("B00110", "B", 1, 10)]
    [InlineData("B00320", "B", 3, 20)]
    public void TranslatingPoints(string code, string tokenIdentifier, int baseNumber, int pointValue)
    {   
        var pointTranslationResult = CodeTranslator.TranslateCoinPointCode(code);
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
        var scoutTranslationResult = CodeTranslator.TranslateScoutCode(code);
        
        scoutTranslationResult.TokenIdentifier.ShouldBe(tokenIdentifier);
        scoutTranslationResult.Section.ShouldBe(section);
        scoutTranslationResult.TroopNumber.ShouldBe(troop);
        scoutTranslationResult.ScoutNumber.ShouldBe(memberNumber);
    }
}