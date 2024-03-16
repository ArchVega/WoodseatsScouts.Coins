// dotcover disable
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.View;

public class AddPointsToMemberViewModel
{
    public AddPointsToMemberViewModel(List<Coin> alreadyScavengedCoins)
    {
        Result = new Dictionary<string, object>();

        if (alreadyScavengedCoins.Count == 0) return;

        Result.Add("Message", "Coins already scavenged!");
        Result.Add("Coins", alreadyScavengedCoins.Select(x => new
        {
            CoinCode = x.Code,
            MemberName = x.Member!.FullName
        }));
    }

    public Dictionary<string, object> Result { get; set; }
}