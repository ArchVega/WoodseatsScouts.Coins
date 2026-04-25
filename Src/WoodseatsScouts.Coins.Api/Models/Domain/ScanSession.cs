namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class ScanSession
{
    public int Id { get; set; }
    
    public int ScoutMemberId { get; set; }
    
    public ScoutMember? ScoutMember { get; set; }
    
    public DateTime CompletedAt { get; set; }

    public List<ScannedCoin> ScanCoins { get; set; } = [];
}