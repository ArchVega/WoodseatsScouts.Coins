using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members;

public class ActivityBaseHaulResultDto
{
    public int ActivityBaseId { get; set; }
    public string ActivityBaseName { get; set; }
    public int TotalPoints { get; set; }
    public int CoinsScanned { get; set; }
    public List<CoinDto> Coins { get; set; }
}