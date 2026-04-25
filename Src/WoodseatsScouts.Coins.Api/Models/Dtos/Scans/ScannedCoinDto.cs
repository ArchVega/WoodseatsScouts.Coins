using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scans;

public class ScannedCoinDto
{
    public ScannedCoinDto(ScannedCoin scannedCoin)
    {
        if (scannedCoin.Coin == null)
        {
            throw new InvalidOperationException("Scanned coin must have navigation property Coin loaded");
        }
        
        ScannedCoinId = scannedCoin.Id;
        CoinId = scannedCoin.CoinId;
        Points = scannedCoin.Coin!.Value;

        if (scannedCoin.PointsOverride.HasValue)
        {
            PointsOverride = scannedCoin.PointsOverride.Value;
        }
        
        if (scannedCoin.Coin.ActivityBase == null)
        {
            throw new InvalidOperationException("Coin navigation property on ScannedCoin must have navigation property ActivityBase loaded");
        }

        CalculatedEffectivePoints = scannedCoin.PointsOverride ?? Points;

        HasPointsOverride = PointsOverride.HasValue;
        CoinActivityBase = scannedCoin.Coin.ActivityBase.Name;
    }

    public int ScannedCoinId { get; set; }
    
    public int CoinId { get; set; }
    
    public int Points { get; set; }

    public int? PointsOverride { get; set; }
    
    public int CalculatedEffectivePoints { get; }

    public string CoinActivityBase { get; set; }

    public bool HasPointsOverride { get; }
}