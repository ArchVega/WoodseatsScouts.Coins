namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class ScanSession
{
    public int Id { get; set; }
    
    public int ScoutMemberId { get; set; }
    
    public required ScoutMember ScoutMember { get; set; }
    
    public DateTime CompletedAt { get; set; }

    public required List<ScanCoin> ScanCoins { get; set; } = [];
}