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
        
        public override string ToString()
        {
            return $"{nameof(Coin.Code)}: {Coin!.Code}, {nameof(Coin.Value)}: {Coin.Value}";
        }
    }
}
