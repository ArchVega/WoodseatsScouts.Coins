namespace WoodseatsScouts.Coins.App.Models.Domain
{
    public class ScavengedCoin
    {
        public int Id { get; set; }
        
        public int ScavengeResultId { get; set; }
        
        public ScavengeResult ScavengeResult { get; set; }
        
        public string Code { get; set; }
        
        public int BaseNumber { get; set; }
        
        public int PointValue { get; set; }
    }
}
