using WoodseatsScouts.Coins.Api.Config;

namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IScoutsAppEnvironment
{
    public ScoutsAppMode ScoutsAppMode { get; }
}