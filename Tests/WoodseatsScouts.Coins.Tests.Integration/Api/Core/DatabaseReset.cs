using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Core;

public static class DatabaseReset
{
    public static async Task RecreateAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();
    }
}