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

        public static MemberCodeTranslationResult TranslateMemberCode(string code)
        {
            try
            {
                return new MemberCodeTranslationResult
                {
                    TokenIdentifier = code[..1],
                    Section = code.Substring(4, 1),
                    TroopNumber = Convert.ToInt32(code.Substring(1, 3)),
                    MemberNumber = Convert.ToInt32(code[5..])
                };

            }
            catch (Exception e)
            {
                throw new CodeTranslationException($"Could not translate Member Code '{code}'", e);
            }
        }
    }
}