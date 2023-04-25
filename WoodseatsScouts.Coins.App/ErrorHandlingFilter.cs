using Microsoft.AspNetCore.Mvc.Filters;
using WoodseatsScouts.Coins.App.Data;
using WoodseatsScouts.Coins.App.Models.Domain;

public class ErrorHandlingFilter : ExceptionFilterAttribute
{
    
    public override void OnException(ExceptionContext context)
    {
        var appDbContext = context.HttpContext.RequestServices.GetService<IAppDbContext>();
        appDbContext!.ErrorLogs!.Add(ErrorLog.Log(context.Exception));
        appDbContext.SaveChanges();

        context.ExceptionHandled = false; //optional 
    }
}