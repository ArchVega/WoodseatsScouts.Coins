using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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

        var errorResponse = new ErrorResponse
        {
            Success = false
        };
        
        switch (exception)
        {
            case DbUpdateException dbUpdateException:
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                errorResponse.Message = dbUpdateException.InnerException!.Message;
                break;
            default:
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                errorResponse.Message = exception.Message;
                break;
        }
        
        logger.LogError(exception.Message);
        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
    }
}

internal class ErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
}