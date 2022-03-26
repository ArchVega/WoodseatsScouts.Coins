namespace WoodseatsScouts.Coins.App.AppLogic.Translators;

public class ScoutCodeTranslationResult : TranslationResultBase
{
    public int TroopNumber { get; init; }
    public string? Section { get; init; }
    public int ScoutNumber { get; init; }
}