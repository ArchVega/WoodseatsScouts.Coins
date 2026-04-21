namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;

public class CoinDto(int pointValue, int activityBaseId, string code)
{
    public string Code { get; set; } = code;

    public int ActivityBaseId { get; set; } = activityBaseId;

    public int PointValue { get; set; } = pointValue;
}