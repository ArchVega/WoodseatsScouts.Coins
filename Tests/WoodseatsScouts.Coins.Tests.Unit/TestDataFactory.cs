using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Tests;

public static class TestDataFactory
{
    public static Coin CreateCoin(int value)
    {
        return new Coin
        {
            Value = value,
            ActivityBase = null
        };
    }

    public static ScoutGroup CreateScoutGroup(string name)
    {
        return new ScoutGroup
        {
            Name = name 
        };
    }

    public static ScoutSection CreateScoutSection(string name)
    {
        return new ScoutSection
        {
            Name = name
        };
    }
}