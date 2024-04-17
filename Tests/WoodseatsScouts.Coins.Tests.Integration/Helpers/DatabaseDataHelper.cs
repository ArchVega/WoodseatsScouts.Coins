using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Tests.Integration.Helpers;

public class DatabaseDataHelper
{
    private readonly IAppDbContext appDbContext;

    public DatabaseDataHelper(IAppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }
}