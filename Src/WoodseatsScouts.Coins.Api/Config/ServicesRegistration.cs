using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;

namespace WoodseatsScouts.Coins.Api.Config;

public static class ServicesRegistration
{
    public static void RegisterAll(IServiceCollection serviceCollection, IConfiguration configuration, IWebHostEnvironment environment)
    {
        RegisterTransients(serviceCollection);
        RegisterScoped(serviceCollection, configuration, environment);
        RegisterSingletons(serviceCollection);
    }

    private static void RegisterTransients(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IAppDbContext, AppDbContext>();
    }

    private static void RegisterScoped(IServiceCollection serviceCollection, IConfiguration configuration, IHostEnvironment environment)
    {
        serviceCollection.AddDbContext<AppDbContext>((_, options) =>
        {
            const string dbName = "WoodseatsScouts.Coins";
            var connectionString = configuration.GetConnectionString(dbName);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Could not get a connection string value for {dbName} for environment {environment.EnvironmentName}");
            }

            options.UseSqlServer(connectionString);
        });
    }
    
    private static void RegisterSingletons(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<AppConfig>();
        serviceCollection.AddSingleton<IScoutsAppEnvironment, ScoutsAppEnvironmentMode>();
    }
}