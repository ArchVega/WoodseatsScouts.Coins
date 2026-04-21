namespace WoodseatsScouts.Coins.Api.AppLogic.Translators
{
    public class CoinPointTranslationResult : TranslationResultBase 
    {
        public int ActivityBaseSequenceNumber { get; set; }
        
        public int ActivityBaseId { get; init; }

        public int PointValue { get; init; }
    }
}