using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Data;

public static class DbContextSeedFactory
{
    public static void CreateUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Troop>().HasData(new Troop()
        {
            Id = 280, Name = "Norton"
        });
        modelBuilder.Entity<Troop>().HasData(new Troop()
        {
            Id = 999, Name = "Woodseats Explorers"
        });
        modelBuilder.Entity<Troop>().HasData(new Troop()
        {
            Id = 74, Name = "Oak Street"
        });
        
        modelBuilder.Entity<Member>().HasData(new Member
        {
            Id = 1, Code = "M013B001", FirstName = "Conner", LastName = "Gillespie", Number = 1, TroopId = 999,
            Section = "B", HasImage = true
        });
        modelBuilder.Entity<Member>().HasData(new Member
        {
            Id = 2, Code = "M014B002", FirstName = "Orlando", LastName = "Mendez", Number = 2, TroopId = 999,
            Section = "B", HasImage = true
        });
        modelBuilder.Entity<Member>().HasData(new Member
        {
            Id = 3, Code = "M015B003", FirstName = "Calvin", LastName = "Fields", Number = 3, TroopId = 999,
            Section = "B", HasImage = true
        });
        modelBuilder.Entity<Member>().HasData(new Member
        {
            Id = 4, Code = "M016C004", FirstName = "Dillon", LastName = "Durham", Number = 4, TroopId = 74,
            Section = "C", IsDayVisitor = true, HasImage = true
        });
        modelBuilder.Entity<Member>().HasData(new Member
        {
            Id = 5, Code = "M017C005", FirstName = "Josiah", LastName = "Castaneda", Number = 5, TroopId = 74,
            Section = "C", IsDayVisitor = true, HasImage = true
        });
    }
}