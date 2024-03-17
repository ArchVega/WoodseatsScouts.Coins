using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Models;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Data;

public interface IAppDbContext
{
    public DbSet<ScavengedCoin>? ScavengedCoins { get; set; }

    public DbSet<Member>? Members { get; set; }
    
    public DbSet<ScavengeResult>? ScavengeResults { get; set; }
    
    public DbSet<Troop>? Troops { get; set; }
    
    public DbSet<Section>? Sections { get; set; }
    
    public DbSet<Coin>? Coins { get; set; }
    
    public DbSet<ErrorLog>? ErrorLogs { get; set; }

    int SaveChanges();
    
    int GenerateNextMemberCode(int troopId, string section);
    
    List<Member> GetLastThreeUsersToScanPoints();
    
    List<GroupPoints> GetTopThreeGroupsInLastHour();
    
    List<GroupPoints> GetGroupsWithMostPoints();

    Troop CreateTroop(int id, string name);
    
    ScavengeResult CreateScavengeResult(Member member);
    
    void CreateScavengedCoins(ScavengeResult scavengeResult, List<string> coinCodes);
    
    List<Coin> RecordMemberAgainstUnscavengedCoins(Member member, List<string> coinCodes);
}