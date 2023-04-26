using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.App.Models;
using WoodseatsScouts.Coins.App.Models.Domain;

namespace WoodseatsScouts.Coins.App.Data;

public interface IAppDbContext
{
    public DbSet<ScavengedCoin>? ScavengedCoins { get; set; }

    public DbSet<Member>? Members { get; set; }
    
    public DbSet<ScavengeResult>? ScavengeResults { get; set; }
    
    public DbSet<Troop>? Troops { get; set; }
    
    public DbSet<ErrorLog>? ErrorLogs { get; set; }

    int SaveChanges();
    
    int GenerateNextMemberCode(int troopId, string section);
    
    List<Member> GetLastThreeUsersToScanPoints();
    
    List<GroupPoints> GetTopThreeGroupsInLastHour();
    
    List<GroupPoints> GetGroupsWithMostPointsThisWeekend();
}