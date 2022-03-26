namespace WoodseatsScouts.Coins.App.Models.Domain
{
    /// <summary>
    /// Scout photos are stored on disk in the wwwroot/images directory and the files must be a jpg file whose name
    /// matches the ScoutNumber field
    /// </summary>
    public class Scout
    {
        public int Id { get; set; }

        public int ScoutNumber { get; set; }
        
        public string Name { get; set; }
        
        public int TroopNumber { get; init; }
        
        public string? Section { get; init; }
    }
}
