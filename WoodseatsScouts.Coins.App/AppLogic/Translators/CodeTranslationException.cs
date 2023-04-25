namespace WoodseatsScouts.Coins.App.AppLogic.Translators;

public class CodeTranslationException : ArgumentException
{
    public CodeTranslationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}