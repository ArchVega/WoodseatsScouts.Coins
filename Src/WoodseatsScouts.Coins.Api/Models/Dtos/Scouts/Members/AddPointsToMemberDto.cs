// dotcover disable

using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class AddPointsToMemberDto
{
    public AddPointsToMemberDto()
    {
    }
    
    public int ScanSessionId { get; set; }

    public bool HasAnomalyOccurred { get; set; }

    public List<CoinAndMember> AffectedCoins { get; set; } = [];

    public int AnomalousCoinsTotalValue { get; set; }

    public AddPointsToMemberDto(int scanSessionId, IReadOnlyCollection<Coin> alreadyScavengedCoins)
    {
        ScanSessionId = scanSessionId;

        HasAnomalyOccurred = alreadyScavengedCoins.Count != 0;

        AffectedCoins = alreadyScavengedCoins.Select(x => new CoinAndMember
        {
            CoinCode = x.Code,
            MemberName = x.Member!.FullName
        }).ToList();

        AnomalousCoinsTotalValue = alreadyScavengedCoins.Sum(x => x.Value);
    }
}