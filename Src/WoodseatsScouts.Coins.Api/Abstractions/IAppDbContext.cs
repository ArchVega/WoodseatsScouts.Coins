using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IAppDbContext
{
    public DbSet<ScannedCoin> ScannedCoins { get; set; }

    public DbSet<ScoutMember> ScoutMembers { get; set; }
    
    public DbSet<ScanSession> ScanSessions { get; set; }
    
    public DbSet<ScoutGroup> ScoutGroups { get; set; }
    
    public DbSet<ScoutSection> ScoutSections { get; set; }
    
    public DbSet<Coin> Coins { get; set; }
    
    public DbSet<ErrorLog> ErrorLogs { get; set; }

    int SaveChanges();
    
    int GenerateNextMemberCode(int scoutGroupId, string section);
    
    List<ScoutMember> GetLastThreeUsersToScanPoints();
    
    List<ScoutMemberPointsSummaryDto> GetLatestScans(int numberOfScans);
    
    ScoutGroup CreateScoutGroup(string name);
    
    ScanSession CreateScavengeResult(ScoutMember scoutMember);
    
    void CreateScavengedCoins(ScanSession scanSession, List<string> coinCodes);
    
    List<Coin> RecordMemberAgainstUnscavengedCoins(ScoutMember scoutMember, List<string> coinCodes);
    
    DbSet<ActivityBase> ActivityBases { get; set; }

    ScoutMember? CreateMember(string firstName, string lastName, int scoutGroupId, string scoutSectionId, bool isDayVisitor);
    
    ScoutMember UpdateMemberName(int memberId, string firstName, string lastName);
    
    List<Coin> CreateCoins(int baseId, int points, int count);
}