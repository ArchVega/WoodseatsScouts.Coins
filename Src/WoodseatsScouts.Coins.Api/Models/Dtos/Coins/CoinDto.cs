namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

public class CoinDto
{
    public CoinDto()
    {
    }
    
    public CoinDto(int pointValue, int activityBaseId, string code)
    {
        Code = code;
        ActivityBaseId = activityBaseId;
        PointValue = pointValue;
    }

    public string Code { get; set; }

    public int ActivityBaseId { get; set; }

    public int PointValue { get; set; }
}