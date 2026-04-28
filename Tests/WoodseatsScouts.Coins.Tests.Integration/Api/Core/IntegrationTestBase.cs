using WoodseatsScouts.Coins.Tests.Integration.Api.Helpers;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Core;

public class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory factory;
    private readonly HttpClient client;

    protected ActivityBasesClient ActivityBasesClient { get; }
    protected CoinsClient CoinsClient { get; }
    protected ScoutMembersClient ScoutMembersClient { get; }

    protected ScoutGroupsClient ScoutGroupsClient { get; }
    protected ScoutSectionsClient ScoutSectionsClient { get; }
    protected ScansClient ScansClient { get; }

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        this.factory = factory;
        client = factory.CreateClient();

        ActivityBasesClient = new ActivityBasesClient(client);
        CoinsClient = new CoinsClient(client);
        ScoutMembersClient = new ScoutMembersClient(client);
        ScoutGroupsClient = new ScoutGroupsClient(client);
        ScoutSectionsClient = new ScoutSectionsClient(client);
        ScansClient = new ScansClient(client);
    }

    public async Task InitializeAsync()
    {
        await DatabaseReset.RecreateAsync(factory.Services);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}