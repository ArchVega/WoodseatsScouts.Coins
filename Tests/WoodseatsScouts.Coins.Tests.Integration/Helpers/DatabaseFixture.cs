using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.PowerShell;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Tests.Integration.Helpers;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class DatabaseFixture
{
    private readonly DbContextOptions<AppDbContext> contextOptions;
    
    private readonly IOptions<LeaderboardSettings> leaderboardSettingsOptions;
    public LeaderboardSettings LeaderboardSettings { get; }

    public AppDbContext AppDbContext => new(contextOptions, leaderboardSettingsOptions);

    private const string SourceDatabaseConnectionString 
        = "Server=(local);Database=WoodseatsScouts.Coins.Tests.Source;Trusted_Connection=true;TrustServerCertificate=true";

    private const string TestDatabaseConnectionString 
        = "Server=(local);Database=WoodseatsScouts.Coins.Tests.Integration;Trusted_Connection=true;TrustServerCertificate=true ";

    public DatabaseFixture()
    {
        RecreateDbViaPowerShell();

        contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(TestDatabaseConnectionString).Options;
        LeaderboardSettings = new LeaderboardSettings();
        leaderboardSettingsOptions = Options.Create(LeaderboardSettings);
    }

    private static void RecreateDbViaPowerShell()
    {
        var initialSessionState = InitialSessionState.CreateDefault();
        initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;
        
        using var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
        runspace.Open();
        runspace.SessionStateProxy.Path.SetLocation(@"..\..\..\..\..");

        using (var instance = PowerShell.Create(runspace))
        {
            instance.AddCommand("./Utilities/Database/RecreateDbs-2-IntegrationTests.ps1");
            /* Examples of how to hook into the powershell streams. */
            // instance.Streams.Verbose.DataAdded += ConsumeStreamOutput;
            // instance.Streams.Information.DataAdded += ConsumeStreamOutput;
            var results = instance.Invoke();
            Console.WriteLine(results);
        }

        runspace.Close();
    }
    
    // Todo: Instead of having a dedicated "scriptrunner" file, replace with code that can construct powershell statements.
    public void RestoreBaseTestData()
    {
        var initialSessionState = InitialSessionState.CreateDefault();
        initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;
        
        using var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
        runspace.Open();
        runspace.SessionStateProxy.Path.SetLocation(@"..\..\..\..\..");

        using (var instance = PowerShell.Create(runspace))
        {
            instance.Streams.Error.DataAdded += ConsumeErrorStreamOutput;
            instance.AddCommand(@"./Utilities/Database/RestoreBaseTestDataScriptRunner.ps1");
            var results = instance.Invoke();
            Console.WriteLine(results);
        }

        runspace.Close();
    }

    private void ConsumeErrorStreamOutput(object? sender, DataAddedEventArgs e)
    {
        throw new Exception(sender.ToString());
    }
}