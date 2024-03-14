using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Tests.Integration.Helpers;

public class DatabaseDataHelper
{
    private readonly IAppDbContext appDbContext;

    public DatabaseDataHelper(IAppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }
    
    public void CreateMember(int memberNumber, int troopId, string sectionId)
    {
        appDbContext.Members!.Add(new Member
        {
            Number = memberNumber,
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            TroopId = troopId,
            SectionId = sectionId
        });
        
        appDbContext.SaveChanges();
    }

    public Troop CreateTroop(string name)
    {
        var troop = new Troop
        {
            Name = name
        };
        
        appDbContext.Troops!.Add(troop);
        
        appDbContext.SaveChanges();

        return troop;
    }
    
    public void ResetAll()
    {
        appDbContext.Sections!.RemoveRange(appDbContext.Sections.ToList());
        appDbContext.SaveChanges();
        appDbContext.Troops!.RemoveRange(appDbContext.Troops.ToList());
        appDbContext.SaveChanges();
        appDbContext.Members!.RemoveRange(appDbContext.Members.ToList());
        appDbContext.SaveChanges();
        appDbContext.ScavengedCoins!.RemoveRange(appDbContext.ScavengedCoins.ToList());
        appDbContext.SaveChanges();
        appDbContext.ScavengeResults!.RemoveRange(appDbContext.ScavengeResults.ToList());
        appDbContext.SaveChanges();
    }
}