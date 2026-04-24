namespace WoodseatsScouts.Coins.Api.Models.Domain
{
    // dotcover disable
    public class ScanCoin
    {
        public int Id { get; set; }
        
        public int ScanSessionId { get; set; }
        
        public required ScanSession ScanSession { get; set; }
        
        public int CoinId { get; set;  }
        
        public required Coin Coin { get; set; }
        
        public override string ToString()
        {
            return $"{nameof(Coin.Code)}: {Coin.Code}, {nameof(Coin.Value)}: {Coin.Value}";
        }
    }
}
