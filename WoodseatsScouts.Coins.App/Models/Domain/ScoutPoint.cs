namespace WoodseatsScouts.Coins.App.Models.Domain
{
    public class ScoutPoint
    {
        public int Id { get; set; }
        
        public int ScoutId { get; set; }
        
        public Scout Scout { get; set; }
        
        public string ScannedCode { get; set; }
        
        public int BaseNumber { get; set; }
        
        public int PointValue { get; set; }
    }
}
