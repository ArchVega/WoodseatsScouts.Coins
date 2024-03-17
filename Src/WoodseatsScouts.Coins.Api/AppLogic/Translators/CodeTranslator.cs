namespace WoodseatsScouts.Coins.Api.AppLogic.Translators
{
    public static class CodeTranslator
    {
        public static CoinPointTranslationResult TranslateCoinCode(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && code[..1] == "M")
            {
                throw new CodeTranslationException($"The code '{code}' is a Member code");                
            }
            
            try
            {
                return new CoinPointTranslationResult
                {
                    TokenIdentifier = code[..1],
                    Id = Convert.ToInt32(code.Substring(1, 4)),
                    BaseNumber = Convert.ToInt32(code.Substring(5, 3)),
                    PointValue = Convert.ToInt32(code[8..])
                };
            }
            catch (Exception e)
            {
                throw new CodeTranslationException($"Could not translate Coin Code '{code}'", e);
            }
        }

        public static MemberCodeTranslationResult TranslateMemberCode(string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && code[..1] == "C")
            {
                throw new CodeTranslationException($"The code '{code}' is a Coin code");                
            }
            
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