﻿namespace WoodseatsScouts.Coins.Api.AppLogic.Translators;

public class MemberCodeTranslationResult : TranslationResultBase
{
    public int TroopNumber { get; init; }
    public string? Section { get; init; }
    public int MemberNumber { get; init; }
}