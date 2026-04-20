using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Models;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;
using WoodseatsScouts.Coins.Api.Models.View.Members;

namespace WoodseatsScouts.Coins.Api.Data;

public interface IAppDbContext
{
    public DbSet<ScavengedCoin>? ScavengedCoins { get; set; }

    public DbSet<Member>? Members { get; set; }
    
    public DbSet<ScavengeResult>? ScavengeResults { get; set; }
    
    public DbSet<ScoutGroup>? ScoutGroups { get; set; }
    
    public DbSet<Section>? Sections { get; set; }
    
    public DbSet<Coin>? Coins { get; set; }
    
    public DbSet<ErrorLog>? ErrorLogs { get; set; }

    int SaveChanges();
    
    int GenerateNextMemberCode(int scoutGroupId, string section);
    
    List<Member> GetLastThreeUsersToScanPoints();
    
    List<MemberPointsSummaryDto> GetLatestScans(int numberOfScans);
    
    List<GroupPoints> GetTopThreeGroupsInLastHour();
    
    List<GroupPoints> GetGroupsWithMostPoints();

    ScoutGroup CreateScoutGroup(int id, string name);
    
    ScavengeResult CreateScavengeResult(Member member);
    
    void CreateScavengedCoins(ScavengeResult scavengeResult, List<string> coinCodes);
    
    List<Coin> RecordMemberAgainstUnscavengedCoins(Member member, List<string> coinCodes);
    
    DbSet<ActivityBase> ActivityBases { get; set; }

    Member? CreateMember(string firstName, string lastName, int scoutGroupId, string section, bool isDayVisitor);
    
    Member UpdateMemberName(int memberId, string firstName, string lastName);
    
    List<Coin> CreateCoins(int baseId, int points, int count);
}