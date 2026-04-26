namespace WoodseatsScouts.Coins.Api.AppLogic.Translators;

public class MemberCodeTranslationResult : TranslationResultBase
{
    public int ScoutGroupId { get; init; }
    
    public string? ScoutSectionCode { get; init; }
    
    public int ScoutMemberNumber { get; init; }
}