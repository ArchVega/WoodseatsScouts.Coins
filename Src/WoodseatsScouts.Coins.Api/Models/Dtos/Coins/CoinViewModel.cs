namespace WoodseatsScouts.Coins.Api.Controllers;

public class CoinViewModel(int pointValue, int baseNumber, string code)
{
    public string Code { get; set; } = code;

    public int BaseNumber { get; set; } = baseNumber;

    public int PointValue { get; set; } = pointValue;
}