namespace WoodseatsScouts.Coins.App.AppLogic.Translators
{
    public static class CodeTranslator
    {
        public static CoinPointTranslationResult TranslateCoinPointCode(string code)
        {
            return new CoinPointTranslationResult
            {
                TokenIdentifier = code[..1],
                BaseNumber = Convert.ToInt32(code.Substring(1, 3)),
                PointValue = Convert.ToInt32(code[4..])
            };
        }

        public static ScoutCodeTranslationResult TranslateScoutCode(string code)
        {
            return new ScoutCodeTranslationResult
            {
                TokenIdentifier = code[..1],
                Section = code.Substring(4, 1),
                TroopNumber = Convert.ToInt32(code.Substring(1, 3)),
                ScoutNumber = Convert.ToInt32(code[5..])
            };
        }
    }
}