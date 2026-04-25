// dotcover disable

using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class AddPointsToMemberDto
{
    public bool HasAnomalyOccurred { get; set; }

    public dynamic AffectedCoins { get; set; }
    
    public int AnomalousCoinsTotalValue { get; set; }

    public AddPointsToMemberDto(IReadOnlyCollection<Coin> alreadyScavengedCoins)
    {
        HasAnomalyOccurred = alreadyScavengedCoins.Count != 0;

        AffectedCoins = alreadyScavengedCoins.Select(x => new
        {
            CoinCode = x.Code,
            MemberName = x.Member!.FullName
        }).ToList();

        AnomalousCoinsTotalValue = alreadyScavengedCoins.Sum(x => x.Value);
    }
}