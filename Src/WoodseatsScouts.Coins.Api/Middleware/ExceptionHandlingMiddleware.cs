using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Middleware;

// dotcover disable
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext, AppDbContext appDbContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex, appDbContext);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, AppDbContext appDbContext)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        string errorResponse;
        
        switch (exception)
        {
            case InvalidOperationException invalidOperationException:
                if (invalidOperationException.Message.Contains("Sequence contains no elements"))
                {
                    // todo: throw specific exception type
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = "Oops, we can't find your profile - please speak to a District Camp Leader";
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = "Oops, an unusual error occurred - please speak to a District Camp Leader";
                }
                break;
            case CodeTranslationException codeTranslationException:
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                errorResponse = codeTranslationException.Message;
                break;
            case DbUpdateException dbUpdateException:
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                errorResponse = dbUpdateException.InnerException!.Message;
                break;
            default:
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                errorResponse = exception.Message;
                break;
        }
        
        logger.LogError(exception.Message);
        try
        {
            appDbContext.ErrorLogs.Add(new ErrorLog
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Path = context.Request.Path,
                Method = context.Request.Method,
                LoggedAt = DateTime.UtcNow
            });
            await appDbContext.SaveChangesAsync();
        }
        catch (Exception dbEx)
        {
            logger.LogError(dbEx.Message);
        }
        
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}