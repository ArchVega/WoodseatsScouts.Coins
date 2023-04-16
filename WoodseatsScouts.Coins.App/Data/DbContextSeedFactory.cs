using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Data;

public static class DbContextSeedFactory
{
    public static void CreateUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Scout>().HasData(new Scout
            {Id = 1, Name = "Scout A", ScoutNumber = 4, TroopNumber = 13, Section = "B"});
        modelBuilder.Entity<Scout>().HasData(new Scout
            {Id = 2, Name = "Scout B", ScoutNumber = 5, TroopNumber = 13, Section = "B"});
        modelBuilder.Entity<Scout>().HasData(new Scout
            {Id = 3, Name = "Scout C", ScoutNumber = 8, TroopNumber = 13, Section = "B"});
        modelBuilder.Entity<Scout>().HasData(new Scout
            {Id = 4, Name = "Scout D", ScoutNumber = 10, TroopNumber = 16, Section = "C"});
        modelBuilder.Entity<Scout>().HasData(new Scout
            {Id = 5, Name = "Scout E", ScoutNumber = 19, TroopNumber = 16, Section = "C"});
    }
}