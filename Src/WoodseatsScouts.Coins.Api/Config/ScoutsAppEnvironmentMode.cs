using WoodseatsScouts.Coins.Api.Abstractions;

namespace WoodseatsScouts.Coins.Api.Config;

public class ScoutsAppEnvironmentMode : IScoutsAppEnvironment
{
    public ScoutsAppMode ScoutsAppMode
    {
        get
        {
            const string key = AppConsts.EnvironmentVariables.ScoutsApiAppMode;
            var envVariableValue = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrEmpty(envVariableValue))
            {
                return ScoutsAppMode.Production;
            }

            var parsed = Enum.TryParse(envVariableValue, out ScoutsAppMode scoutsAppMode);

            if (parsed)
            {
                return scoutsAppMode;
            }

            throw new Exception($"Could not parse value '{envVariableValue}' for environment variable '{key}'");
        }
    }
}