using System.ComponentModel.DataAnnotations.Schema;

namespace WoodseatsScouts.Coins.Api.Models.Domain
{
    // dotcover disable
    public class ScannedCoin
    {
        public int Id { get; set; }
        
        public int ScanSessionId { get; set; }
        
        public ScanSession? ScanSession { get; set; }
        
        public int CoinId { get; set;  }
        
        public Coin? Coin { get; set; }

        public int? PointsOverride { get; set; }
        
        [NotMapped]
        public bool HasPointsOverride => PointsOverride.HasValue;

        [NotMapped]
        public int? CalculatedEffectivePoints
        {
            get
            {
                if (Coin != null)
                {
                    return PointsOverride ?? Coin.Value;
                }

                return null;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Coin.Code)}: {Coin!.Code}, {nameof(Coin.Value)}: {Coin.Value}";
        }
    }
}
