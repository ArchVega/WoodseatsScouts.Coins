using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WoodseatsScouts.Coins.App.Models.Domain;

public class ErrorLog
{
    public int Id { get; set; }
    
    public DateTime LoggedAt { get; set; }
    
    public string Message { get; set; }
    
    public string? StackTrace { get; set; }

    public static ErrorLog Log(Exception e)
    {
        return new ErrorLog()
        {
            LoggedAt = DateTime.Now,
            Message = e.Message,
            StackTrace = e.StackTrace
        };
    }
}