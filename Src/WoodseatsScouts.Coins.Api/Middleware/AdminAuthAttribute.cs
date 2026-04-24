using Microsoft.AspNetCore.Mvc;

namespace WoodseatsScouts.Coins.Api.Middleware;

public class AdminAuthAttribute() : TypeFilterAttribute(typeof(AdminAuthFilter));