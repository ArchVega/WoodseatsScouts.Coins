namespace WoodseatsScouts.Coins.Api.AppLogic.Translators;

public class CodeTranslationException : ArgumentException
{
    public CodeTranslationException(string? message) : base(message)
    {
    }
    
    public CodeTranslationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}