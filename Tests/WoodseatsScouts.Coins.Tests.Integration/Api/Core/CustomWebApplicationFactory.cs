using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Core;

// ReSharper disable once ClassNeverInstantiated.Global
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string ConnectionString 
        = "Server=localhost,1433;Database=WoodseatsScouts.Coins.Tests.Integration;User Id=SA;Password=Pa55w0rd123;TrustServerCertificate=True;Encrypt=False";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var settings = new Dictionary<string, string>
            {
                ["ConnectionStrings:WoodseatsScouts.Coins"] = ConnectionString
            };

            config.AddInMemoryCollection(settings!);
        });
    }
}