using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WoodseatsScouts.Coins.Api.Config;

namespace WoodseatsScouts.Coins.Api.Middleware;

public class AdminAuthFilter(IConfiguration configuration) : IAuthorizationFilter
{
    private readonly string _token = configuration["AppSettings:AuthenticationToken"]!;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var headers = context.HttpContext.Request.Headers;
        if (!headers.TryGetValue(AppSettings.ApiAuthenticationTokenKey, out var providedToken) || _token != providedToken)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}