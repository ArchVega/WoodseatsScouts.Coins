namespace WoodseatsScouts.Coins.Api.AppLogic.Translators
{
    public class CoinPointTranslationResult : TranslationResultBase 
    {
        public int Id { get; set; }
        
        public int BaseNumber { get; init; }

        public int PointValue { get; init; }
    }
}