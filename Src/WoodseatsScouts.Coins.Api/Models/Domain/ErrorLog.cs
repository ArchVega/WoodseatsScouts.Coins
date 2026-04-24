using System.Diagnostics.CodeAnalysis;

namespace WoodseatsScouts.Coins.Api.Models.Domain;

// dotcover disable
[SuppressMessage("ReSharper", "EntityFramework.ModelValidation.UnlimitedStringLength")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class ErrorLog
{
    public int Id { get; set; }
    
    public DateTime LoggedAt { get; set; }
    
    public required string Message { get; set; }
    
    public string? StackTrace { get; set; }
    
    public required string Path { get; set; }
    
    public required string Method { get; set; }
}