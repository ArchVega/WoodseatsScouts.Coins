using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.App.Data;

namespace WoodseatsScouts.Coins.Tests.Integration;

public class DatabaseFixture : IDisposable
{
    public readonly AppDbContext AppDbContext;

    public DatabaseFixture()
    {
        const string connectionString =
            "Server=DESKTOP-25Q8KUA;Database=WoodseatsScouts.Coins.Tests;Trusted_Connection=true";
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString).Options;
        AppDbContext = new AppDbContext(contextOptions);
        
        AppDbContext.Troops!.RemoveRange(AppDbContext.Troops.ToList());
        AppDbContext.SaveChanges();
        AppDbContext.Members!.RemoveRange(AppDbContext.Members.ToList());
        AppDbContext.SaveChanges();

        AppDbContext.Troops!.AddRange(TestDataFactory.GetKnownTroops());
        AppDbContext.SaveChanges();

        AppDbContext.Members!.AddRange(TestDataFactory.GetKnownMembers());
        AppDbContext.SaveChanges();

        var k = 17;
        for (int i = 5; i < 26; i++)
        {
            var troop = TestDataFactory.CreateTroop(i);
            AppDbContext.Troops.Add(troop);
            AppDbContext.SaveChanges();
            for (int j = 0; j < 10; j++)
            {
                var member = TestDataFactory.CreateTroopMember(k++, troop, 'Z');
                AppDbContext.Members.Add(member);
            }

            AppDbContext.SaveChanges();
        }
    }

    public void Dispose()
    {
        // ... clean up test data from the database ...
    }

    public void ResetCoins()
    {
        AppDbContext.ScavengedCoins!.RemoveRange(AppDbContext.ScavengedCoins.ToList());
        AppDbContext.SaveChanges();
        AppDbContext.ScavengeResults!.RemoveRange(AppDbContext.ScavengeResults.ToList());
        AppDbContext.SaveChanges();
    }
}