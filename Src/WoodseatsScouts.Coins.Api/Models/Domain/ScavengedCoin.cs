namespace WoodseatsScouts.Coins.Api.Models.Domain
{
    // dotcover disable
    public class ScavengedCoin
    {
        public int Id { get; set; }
        
        public int ScavengeResultId { get; set; }
        
        public ScavengeResult ScavengeResult { get; set; }
        
        public int CoinId { get; set;  }
        
        public Coin Coin { get; set; }
        
        // public string Code { get; set; }
        //
        // public int BaseNumber { get; set; }
        //
        // public int PointValue { get; set; }

        public override string ToString()
        {
            return $"{nameof(Coin.Code)}: {Coin.Code}, {nameof(Coin.Value)}: {Coin.Value}";
        }
    }
}
