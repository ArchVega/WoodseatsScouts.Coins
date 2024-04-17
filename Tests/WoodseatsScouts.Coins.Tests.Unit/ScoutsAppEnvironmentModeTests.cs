using System;
using Shouldly;
using WoodseatsScouts.Coins.Api;
using WoodseatsScouts.Coins.Api.Config;
using Xunit;

namespace WoodseatsScouts.Coins.Tests;

public class ScoutsAppEnvironmentModeTests
{
    [Theory]
    [InlineData("Development", ScoutsAppMode.Development)]
    [InlineData("AcceptanceTest", ScoutsAppMode.AcceptanceTest)]
    [InlineData("Production", ScoutsAppMode.Production)]
    public void EnvironmentVariableSetToValidValue_EnumGeneratedAsExpected(string value, ScoutsAppMode expected)
    {
        try
        {
            Environment.SetEnvironmentVariable(AppConsts.EnvironmentVariables.ScoutsApiAppMode, value);

            var cut = new ScoutsAppEnvironmentMode();
            var result = cut.ScoutsAppMode;

            result.ShouldBe(expected);
        }
        finally
        {
            Environment.SetEnvironmentVariable(AppConsts.EnvironmentVariables.ScoutsApiAppMode, null);
        }
    }
    
    [Theory]
    [InlineData("development")]
    [InlineData("acceptance-testing")]
    public void EnvironmentVariableSetToInvalidValue_ExceptionThrown(string value)
    {
        Environment.SetEnvironmentVariable(AppConsts.EnvironmentVariables.ScoutsApiAppMode, value);
        
        var cut = new ScoutsAppEnvironmentMode();
        
        var ex = Should.Throw<Exception>(() => cut.ScoutsAppMode);
        
        ex.Message.ShouldBe($"Could not parse value '{value}' for environment variable 'SCOUTS_API_APP_MODE'");
    }

    [Fact]
    public void NoValueProvided_DefaultsToProduction()
    {
        Environment.SetEnvironmentVariable(AppConsts.EnvironmentVariables.ScoutsApiAppMode, null);
        
        var cut = new ScoutsAppEnvironmentMode();

        cut.ScoutsAppMode.ShouldBe(ScoutsAppMode.Production);
    }
}