using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.AppLogic.Translators;

namespace WoodseatsScouts.Coins.Api.Middleware;

// dotcover disable
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        string errorResponse;
        
        switch (exception)
        {
            case InvalidOperationException invalidOperationException:
                if (invalidOperationException.Message.Contains("Sequence contains no elements"))
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = "Could not find the requested resource";
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = $"Error of type {invalidOperationException.GetType().Name} not handled";
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
        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
    }
}