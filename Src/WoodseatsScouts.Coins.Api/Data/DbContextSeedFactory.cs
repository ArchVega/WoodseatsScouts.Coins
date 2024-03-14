using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Data;

// dotcover disable
public static class DbContextSeedFactory
{
    public static void CreateSections(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Section>().HasData(new Section("B", "Beavers"));
        modelBuilder.Entity<Section>().HasData(new Section("C", "Cubs"));
        modelBuilder.Entity<Section>().HasData(new Section("E", "Explorers"));
        modelBuilder.Entity<Section>().HasData(new Section("A", "Adults"));
        modelBuilder.Entity<Section>().HasData(new Section("S", "Scouts"));

#if DEBUG
        modelBuilder.Entity<Troop>().HasData(new Troop() { Id = 1, Name = "Charcoal" });
        modelBuilder.Entity<Troop>().HasData(new Troop() { Id = 2, Name = "Jet" });
        modelBuilder.Entity<Troop>().HasData(new Troop() { Id = 3, Name = "Hunter" });

        modelBuilder.Entity<Member>().HasData(new Member
            { Id = 1, FirstName = "Crimson", LastName = "Charcoal", SectionId = "A", TroopId = 1, HasImage = true });
        modelBuilder.Entity<Member>().HasData(new Member
            { Id = 5, FirstName = "Glaucous", LastName = "Jet", SectionId = "B", TroopId = 2, HasImage = true });
        modelBuilder.Entity<Member>().HasData(new Member
            { Id = 13, FirstName = "Saffron", LastName = "Hunter", SectionId = "C", TroopId = 3, HasImage = true });

        var coinId = 1;
        foreach (var i in Enumerable.Range(1, 10))
        {
            foreach (var coinValue in new List<int> { 3, 9, 10, 11, 20 })
            {
                var baseNumber = (coinId % 3) + 1;
                modelBuilder.Entity<Coin>().HasData(new Coin
                    { Id = coinId++, Base = baseNumber, Value = coinValue, Code = $"B{coinId:0000}{baseNumber:000}{coinValue:000}" });
            }    
        }
#endif
    }
}