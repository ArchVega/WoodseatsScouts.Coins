using Shouldly;
using WoodseatsScouts.Coins.Api.Models.Dtos.ActivityBases;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;
using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;
using WoodseatsScouts.Coins.Tests.Integration.Api.Core;

namespace WoodseatsScouts.Coins.Tests.Integration.Api.Tests.Scenarios;

public class ActivityBaseCoinHelper
{
    public readonly Dictionary<ActivityBasesDto, List<Tuple<CoinDto, bool>>> Lookup = new();

    public CoinDto? GetNextAvailableCoin(ActivityBasesDto activityBasesDto)
    {
        return Lookup[activityBasesDto].FirstOrDefault(x => !x.Item2)?.Item1;
    }
}

public class ScenarioActivityBaseSummariesAreCorrectTest(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Test()
    {
        var archeryBase = await ActivityBasesClient.PostActivityBases("Archery");
        var abseiling = await ActivityBasesClient.PostActivityBases("Abseiling");
        var aeroball = await ActivityBasesClient.PostActivityBases("Aeroball");
        var bushcraft = await ActivityBasesClient.PostActivityBases("Bushcraft");
        var fencing = await ActivityBasesClient.PostActivityBases("Fencing");
        var powerboating = await ActivityBasesClient.PostActivityBases("Powerboating");
        var zipWire = await ActivityBasesClient.PostActivityBases("Zip wire");
        var activityBaseDtos = new List<ActivityBasesDto> { archeryBase, abseiling, aeroball, bushcraft, fencing, powerboating, zipWire };

        var scoutSectionA = await ScoutSectionsClient.PostScoutSection("c", "Any");
        var scoutGroupAlpha = await ScoutGroupsClient.PostScoutGroup("Group 1");

        var keanuReeves = await ScoutMembersClient.PostScoutMember("Keanu", "Reeves", scoutGroupAlpha.Id, scoutSectionA.Code);
        var tomHanks = await ScoutMembersClient.PostScoutMember("Tom", "Hanks", scoutGroupAlpha.Id, scoutSectionA.Code);
        var steveIrwin = await ScoutMembersClient.PostScoutMember("Steve", "Irwin", scoutGroupAlpha.Id, scoutSectionA.Code);
        var jackieChan = await ScoutMembersClient.PostScoutMember("Jackie", "Chan", scoutGroupAlpha.Id, scoutSectionA.Code);
        var denzelWashington = await ScoutMembersClient.PostScoutMember("Denzel", "Washington", scoutGroupAlpha.Id, scoutSectionA.Code);
        var bettyWhite = await ScoutMembersClient.PostScoutMember("Betty", "White", scoutGroupAlpha.Id, scoutSectionA.Code);
        var bobRoss = await ScoutMembersClient.PostScoutMember("Bob", "Ross", scoutGroupAlpha.Id, scoutSectionA.Code);
        var morganFreeman = await ScoutMembersClient.PostScoutMember("Morgan", "Freeman", scoutGroupAlpha.Id, scoutSectionA.Code);
        var steveCarell = await ScoutMembersClient.PostScoutMember("Steve", "Carell", scoutGroupAlpha.Id, scoutSectionA.Code);
        var davidAttenborough = await ScoutMembersClient.PostScoutMember("David", "Attenborough", scoutGroupAlpha.Id, scoutSectionA.Code);
        var dannyDeVito = await ScoutMembersClient.PostScoutMember("Danny", "DeVito", scoutGroupAlpha.Id, scoutSectionA.Code);
        var samuelLJackson = await ScoutMembersClient.PostScoutMember("Samuel", "L Jackson", scoutGroupAlpha.Id, scoutSectionA.Code);

        var scoutMemberDtos = new List<ScoutMemberDto>
        {
            keanuReeves, tomHanks, steveIrwin, jackieChan, denzelWashington, bettyWhite, bobRoss, morganFreeman, steveCarell, davidAttenborough, dannyDeVito, samuelLJackson,
        };

        foreach (var scoutMemberDto in scoutMemberDtos)
        {
            var data = (await ScoutMembersClient.GetScoutMemberComplete(scoutMemberDto.ScoutGroupId)).Data;
        }

        var activityBaseCoinHelper = new ActivityBaseCoinHelper();
        
        
        foreach (var activityBaseDto in activityBaseDtos)
        {
            var coinDtos = await CoinsClient.CreateCoins(activityBaseDto.Id, 10, 10);
            var tuples = coinDtos.Select(x => new Tuple<CoinDto, bool>(x, false)).ToList();
            activityBaseCoinHelper.Lookup[activityBaseDto] = tuples;
        }
        
        // var scanSession = await ScoutMembersClient.AssignCoinsToScoutMember(keanuReeves.Id, coinDtos.Select(x => x.Code).ToList());
        // scanSession.ScanSessionId.ShouldBe(1);

        int i = 0;
    }
}