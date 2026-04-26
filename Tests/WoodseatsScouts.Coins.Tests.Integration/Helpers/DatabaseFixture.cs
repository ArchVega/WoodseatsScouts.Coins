using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.PowerShell;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Tests.Integration.Helpers;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class DatabaseFixture
{
    private readonly DbContextOptions<AppDbContext> contextOptions;

    private readonly IOptions<AppSettings> appSettingsOptions;
    
    public AppSettings AppSettings { get; set; }

    public AppDbContext AppDbContext => new(contextOptions, appSettingsOptions);

    private const string SourceDatabaseConnectionString 
        = "Server=localhost,1433;Database=WoodseatsScouts.Coins.Development;User Id=SA;Password=Pa55w0rd123;TrustServerCertificate=True;Encrypt=False";

    private const string TestDatabaseConnectionString 
        = "Server=localhost,1433;Database=WoodseatsScouts.Coins.Tests.Integration;User Id=SA;Password=Pa55w0rd123;TrustServerCertificate=True;Encrypt=False";

    public DatabaseFixture()
    {
        RecreateDbViaPowerShell();

        // contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(SourceDatabaseConnectionString).Options;
        contextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(TestDatabaseConnectionString).Options;

        AppSettings = new AppSettings
        {
            AppVersion = "test",
            ContentRootDirectory = "test",
            MinutesToLockScavengedCoins = 10,
            LoginPauseDurationSeconds = 10,
            ParticipantPlaceholderImagePath = "test",
            NumberOfLatestScansToDisplay = 10
        };
        appSettingsOptions = Options.Create(AppSettings);
    }

    private static void RecreateDbViaPowerShell()
    {
        var initialSessionState = InitialSessionState.CreateDefault();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;
        }
        
        using var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
        runspace.Open();
        runspace.SessionStateProxy.Path.SetLocation(@"../../../../..");

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
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;
        }
        
        using var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
        runspace.Open();
        runspace.SessionStateProxy.Path.SetLocation("../../../../..");

        using (var instance = PowerShell.Create(runspace))
        {
            instance.Streams.Error.DataAdded += ConsumeErrorStreamOutput;
            instance.Streams.Verbose.DataAdded += ConsumeErrorStreamOutput;
            instance.Streams.Information.DataAdded += ConsumeStreamOutput;
            instance.AddCommand(@"./Utilities/Database/RestoreBaseTestDataScriptRunner.ps1");
            var results = instance.Invoke();
            Console.WriteLine(results);
        }

        runspace.Close();
    }

    private static void ConsumeStreamOutput(object? sender, DataAddedEventArgs e)
    {
        switch (sender)
        {
            case PSDataCollection<InformationRecord> i:
            {
                var record = i[e.Index];

                var message = record.MessageData?.ToString();
                Console.WriteLine("Information: " + message);
                break;
            }
            case PSDataCollection<VerboseRecord> v:
            {
                var record = v[e.Index];

                var message = record.Message?.ToString();
                Console.WriteLine(message);
                Console.WriteLine("Verbose: " + message);
                break;
            }
        }
    }
    
    private static void ConsumeErrorStreamOutput(object? sender, DataAddedEventArgs e)
    {
        if (sender is not PSDataCollection<ErrorRecord> o) throw new Exception(sender?.ToString());
        
        var errs = o
            .Select(x => x.Exception.Message)
            .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
            
        throw new Exception(errs);

    }
}