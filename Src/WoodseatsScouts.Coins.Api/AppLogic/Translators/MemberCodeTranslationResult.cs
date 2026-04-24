namespace WoodseatsScouts.Coins.Api.AppLogic.Translators;

public class MemberCodeTranslationResult : TranslationResultBase
{
    public int ScoutGroupNumber { get; init; }
    
    public string? SectionCode { get; init; }
    
    public int ScoutMemberNumber { get; init; }
}